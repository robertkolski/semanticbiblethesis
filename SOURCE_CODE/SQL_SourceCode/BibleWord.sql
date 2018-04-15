CREATE TABLE dbo.StrongWord
(
	StrongWordId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	StrongNumber varchar(10),
	StrongWord varchar(64),
	[Definition] varchar(max)
)

CREATE TABLE dbo.BibleWord
(
	BibleWordId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	WordNumber smallint,
	WordText varchar(64),
	BibleVerseId INT NOT NULL CONSTRAINT FK_Verse FOREIGN KEY REFERENCES dbo.BibleVerse(BibleVerseId),
	StrongWordId INT NULL CONSTRAINT FK_StrongWord FOREIGN KEY REFERENCES dbo.StrongWord(StrongWordId)
)
GO

CREATE PROCEDURE dbo.InsertBibleWord
(
	@BibleName varchar(64),
	@BookName varchar(64),
	@BookNumber smallint,
	@ChapterNumber smallint,
	@VerseNumber smallint,
	@WordNumber smallint,
	@WordText smallint,
	@StrongNumber varchar(10)
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


CREATE PROCEDURE dbo.InsertStrongWord
(
	@StrongWordId INT,
	@StrongNumber varchar(10),
	@StrongWord varchar(64),
	@Definition varchar(max)
)
AS
BEGIN
	INSERT INTO dbo.StrongWord
	(
		StrongNumber,
		StrongWord,
		[Definition]
	)
	VALUES
	(
		@StrongNumber,
		@StrongWord,
		@Definition
	)
END
GO