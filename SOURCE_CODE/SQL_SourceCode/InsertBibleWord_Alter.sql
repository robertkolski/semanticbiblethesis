USE [AnalyzeBible_Dev]
GO

/****** Object:  StoredProcedure [dbo].[InsertBibleWord]    Script Date: 5/3/2016 12:41:44 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[InsertBibleWord]
(
	@BibleName varchar(64),
	@BookName varchar(64),
	@BookNumber smallint,
	@ChapterNumber smallint,
	@VerseNumber smallint,
	@WordNumber smallint,
	@WordText varchar(64),
	@StrongNumber varchar(10) = null
)
AS
BEGIN
	DECLARE @BibleEditionId INT
	DECLARE @BibleBookId INT
	DECLARE @BibleChapterId INT
	DECLARE @BibleVerseId INT
	DECLARE @StrongWordId INT
	DECLARE @BibleWordId INT

	SELECT @BibleEditionId = BibleEditionId FROM dbo.BibleEdition WHERE BibleName = @BibleName
	SELECT @BibleBookId = BibleBookId FROM dbo.BibleBook WHERE BookName = @BookName AND BookNumber = @BookNumber AND BibleEditionId = @BibleEditionId
	SELECT @BibleChapterId = BibleChapterId FROM dbo.BibleChapter WHERE ChapterNumber = @ChapterNumber AND BibleBookId = @BibleBookId
	SELECT @BibleVerseId = BibleVerseId FROM dbo.BibleVerse WHERE VerseNumber = @VerseNumber AND BibleChapterId = @BibleChapterId
	SELECT @StrongWordId = StrongWordId FROM dbo.StrongWord WHERE StrongNumber = @StrongNumber
	IF (ISNULL(@BibleVerseId, 0) <> 0) -- defect here
	BEGIN
		SELECT @BibleWordId = BibleWordId FROM dbo.BibleWord WHERE BibleVerseId = @BibleVerseId AND WordNumber = @WordNumber

		IF (ISNULL(@BibleWordId, 0) = 0)
		BEGIN
			INSERT INTO dbo.BibleWord
			(
				BibleVerseId,
				WordNumber,
				WordText,
				StrongWordId
			)
			VALUES
			(
				@BibleVerseId,
				@WordNumber,
				@WordText,
				@StrongWordId
			)
		END
		ELSE
		BEGIN
			UPDATE dbo.BibleWord
			SET WordText = @WordText,
			StrongWordId = @StrongWordId
			WHERE
				WordNumber = @WordNumber
			AND BibleVerseId = @BibleVerseId
		END
	END
END


GO


