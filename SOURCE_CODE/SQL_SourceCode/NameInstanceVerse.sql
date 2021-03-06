/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [NameInstanceVerseId]
      ,[NameInstanceId]
      ,[BibleVerseId]
  FROM [AnalyzeBible_Dev].[dbo].[NameInstanceVerse]

ALTER TABLE dbo.NameInstanceVerse
ADD BibleWordId int NOT NULL CONSTRAINT FK_BibleWord FOREIGN KEY REFERENCES dbo.BibleWord(BibleWordId)
GO


ALTER TABLE dbo.NameInstanceVerse
DROP CONSTRAINT FK_BibleVerse_NIV

ALTER TABLE dbo.NameInstanceVerse
DROP COLUMN BibleVerseId


CREATE PROCEDURE dbo.InsertNameInstanceVerse
(
	@NameInstanceUniqueId varchar(100),
	@BibleName varchar(64),
	@BookName varchar(64),
	@ChapterNumber smallint,
	@VerseNumber smallint,
	@WordNumber smallint
)
AS
BEGIN
	DECLARE @BibleEditionId INT
	DECLARE @BibleBookId INT
	DECLARE @BibleChapterId INT
	DECLARE @BibleVerseId INT
	DECLARE @StrongWordId INT
	DECLARE @BibleWordId INT
	DECLARE @NameInstanceId INT

	SELECT @BibleEditionId = BibleEditionId FROM dbo.BibleEdition WHERE BibleName = @BibleName
	SELECT @BibleBookId = BibleBookId FROM dbo.BibleBook WHERE BookName = @BookName AND BibleEditionId = @BibleEditionId
	SELECT @BibleChapterId = BibleChapterId FROM dbo.BibleChapter WHERE ChapterNumber = @ChapterNumber AND BibleBookId = @BibleBookId
	SELECT @BibleVerseId = BibleVerseId FROM dbo.BibleVerse WHERE VerseNumber = @VerseNumber AND BibleChapterId = @BibleChapterId
	SELECT @BibleWordId = BibleWordId FROM dbo.BibleWord WHERE BibleVerseId = @BibleVerseId AND WordNumber = @WordNumber
	SELECT @NameInstanceId = NameInstanceId FROM dbo.NameInstance WHERE NameInstanceUnqiueId = @NameInstanceUniqueId

	INSERT INTO dbo.NameInstanceVerse
	(
		NameInstanceId,
		BibleWordId
	)
	VALUES
	(
		@NameInstanceId,
		@BibleWordId
	)
END
GO
