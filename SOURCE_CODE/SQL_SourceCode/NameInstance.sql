CREATE TABLE dbo.NameInstance
(
	NameInstanceId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NameId INT NOT NULL CONSTRAINT PK_Name FOREIGN KEY REFERENCES dbo.Name,
	NameInstanceUnqiueId varchar(100)
)

CREATE TABLE dbo.NameInstanceVerse
(
	NameInstanceVerseId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NameInstanceId INT NOT NULL CONSTRAINT FK_NameInstance FOREIGN KEY REFERENCES dbo.NameInstance(NameInstanceId),
	BibleVerseId INT NOT NULL CONSTRAINT FK_BibleVerse_NIV FOREIGN KEY REFERENCES dbo.BibleVerse(BibleVerseId)
)
GO

