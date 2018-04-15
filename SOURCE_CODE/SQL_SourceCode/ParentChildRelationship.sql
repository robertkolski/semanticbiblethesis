CREATE TABLE dbo.ParentChildRelationship
(
	ParentChildRelationshipId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	ParentNameId INT NOT NULL CONSTRAINT FK_ParentName FOREIGN KEY REFERENCES dbo.Name(NameId),
	ChildNameId INT NOT NULL CONSTRAINT FK_ChildName FOREIGN KEY REFERENCES dbo.Name(NameId),
	BibleVerseId INT NULL CONSTRAINT FK_BibleVerse FOREIGN KEY REFERENCES dbo.BibleVerse(BibleVerseId),
	ModerationFlag bit NOT NULL,
	ModerationReason varchar(MAX)
)
GO

CREATE PROCEDURE dbo.InsertParentChild
(
	@ParentName varchar(64),
	@ChildName varchar(64),
	@BibleVerseId int,
	@ModerationFlag bit,
	@ModerationReason varchar(max)
)
AS
BEGIN
	DECLARE @ParentNameId INT
	DECLARE @ChildNameId INT

	SELECT @ParentNameId = NameId FROM dbo.Name WHERE NameText = @ParentName
	SELECT @ChildNameId = NameId FROM dbo.Name WHERE NameText = @ChildName

	INSERT INTO dbo.ParentChildRelationship
	(
		ParentNameId,
		ChildNameId,
		BibleVerseId,
		ModerationFlag,
		ModerationReason
	)
	VALUES
	(
		@ParentNameId,
		@ChildNameId,
		@BibleVerseId,
		@ModerationFlag,
		@ModerationReason
	)
END
GO