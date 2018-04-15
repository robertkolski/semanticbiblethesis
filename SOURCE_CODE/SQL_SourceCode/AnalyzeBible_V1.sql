CREATE DATABASE AnalyzeBible_Dev
GO

USE AnalyzeBible_Dev
GO

CREATE TABLE dbo.BibleEdition
(
	BibleEditionId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	BibleName varchar(64) NOT NULL
)
GO

CREATE TABLE dbo.BibleBook
(
	BibleBookId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	BookNumber SMALLINT NOT NULL,
	BookName varchar(64) NOT NULL,
	BibleEditionId INT NOT NULL CONSTRAINT FK_BibleEdition FOREIGN KEY REFERENCES dbo.BibleEdition(BibleEditionId)
)
GO

CREATE TABLE dbo.BibleChapter
(
	BibleChapterId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	ChapterNumber SMALLINT NOT NULL,
	BibleBookId INT NOT NULL CONSTRAINT FK_BibleBook FOREIGN KEY REFERENCES dbo.BibleBook(BibleBookId)
)
GO

CREATE TABLE dbo.BibleVerse
(
	BibleVerseId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	VerseNumber SMALLINT NOT NULL,
	VerseText varchar(MAX) NOT NULL,
	BibleChapterId INT NOT NULL CONSTRAINT FK_BibleChapter FOREIGN KEY REFERENCES dbo.BibleChapter(BibleChapterId)
)
GO

CREATE PROCEDURE dbo.InsertBibleVerse
(
	@BibleName varchar(64),
	@BookName varchar(64),
	@BookNumber smallint,
	@ChapterNumber smallint,
	@VerseNumber smallint,
	@VerseText varchar(MAX)
)
AS
BEGIN
	DECLARE @BibleEditionId INT
	DECLARE @BibleBookId INT
	DECLARE @BibleChapterId INT
	DECLARE @BibleVerseId INT

	SELECT @BibleEditionId = BibleEditionId FROM dbo.BibleEdition WHERE BibleName = @BibleName

	IF (ISNULL(@BibleEditionId, 0) = 0)
	BEGIN
		INSERT INTO dbo.BibleEdition
		(
			BibleName
		)
		VALUES
		(
			@BibleName
		)
		SET @BibleEditionId = @@IDENTITY
	END

	SELECT @BibleBookId = BibleBookId FROM dbo.BibleBook WHERE BookName = @BookName AND BookNumber = @BookNumber AND BibleEditionId = @BibleEditionId

	IF (ISNULL(@BibleBookId, 0) = 0)
	BEGIN
		INSERT INTO dbo.BibleBook
		(
			BookName,
			BookNumber,
			BibleEditionId
		)
		VALUES
		(
			@BibleName,
			@BookNumber,
			@BibleEditionId
		)
		SET @BibleBookId = @@IDENTITY
	END

	SELECT @BibleChapterId = BibleChapterId FROM dbo.BibleChapter WHERE ChapterNumber = @ChapterNumber AND BibleBookId = @BibleBookId

	IF (ISNULL(@BibleChapterId, 0) = 0)
	BEGIN
		INSERT INTO dbo.BibleChapter
		(
			ChapterNumber,
			BibleBookId
		)
		VALUES
		(
			@ChapterNumber,
			@BibleBookId
		)
		SET @BibleChapterId = @@IDENTITY
	END
	
	SELECT @BibleVerseId = BibleVerseId FROM dbo.BibleVerse WHERE VerseNumber = @VerseNumber AND BibleChapterId = @BibleChapterId

	IF (ISNULL(@BibleChapterId, 0) = 0)
	BEGIN
		INSERT INTO dbo.BibleVerse
		(
			VerseNumber,
			VerseText,
			BibleChapterId
		)
		VALUES
		(
			@VerseNumber,
			@VerseText,
			@BibleChapterId
		)
		SET @BibleVerseId = @@IDENTITY
	END
	ELSE
	BEGIN
		UPDATE dbo.BibleVerse
		SET VerseText = @VerseText
		WHERE
			VerseNumber = @VerseNumber
		AND BibleChapterId = @BibleChapterId
		AND BibleVerseId = @BibleVerseId
	END
END
GO