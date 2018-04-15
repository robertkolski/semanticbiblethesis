USE AnalyzeBible_Dev
GO

CREATE TABLE dbo.RegExCategory
(
	RegExCategoryId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	RegExCategoryName varchar(64) NOT NULL
)
GO

CREATE TABLE dbo.RegExEntry
(
	RegExEntryId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
	RegExText varchar(MAX) NOT NULL,
	RegExCategoryId INT NOT NULL CONSTRAINT FK_RegExCategory FOREIGN KEY REFERENCES dbo.RegExCategory(RegExCategoryId)
)
GO

CREATE PROCEDURE dbo.InsertRegExEntry
(
	@RegExCategoryName varchar(64),
	@RegExText varchar(MAX)
)
AS
BEGIN
	DECLARE @RegExCategoryId INT

	SELECT @RegExCategoryId = RegExCategoryId FROM RegExCategory WHERE RegExCategoryName = @RegExCategoryName

	IF (ISNULL(@RegExCategoryId, 0) = 0)
	BEGIN
		INSERT INTO dbo.RegExCategory
		(
			RegExCategoryName
		)
		VALUES
		(
			@RegExCategoryName
		)

		SET @RegExCategoryId = @@IDENTITY
	END

	IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.RegExEntry WHERE RegExText = @RegExText AND RegExCategoryId = @RegExCategoryId)
	BEGIN
	INSERT INTO dbo.RegExEntry
	(
		RegExText,
		RegExCategoryId
	)
	VALUES
	(
		@RegExText,
		@RegExCategoryId
	)
	END
END
GO

CREATE TABLE dbo.Name
(
	NameText varchar(64) NOT NULL,
	ModerationFlag bit NOT NULL,
	ModerationReason varchar(64)
)
GO

CREATE PROCEDURE dbo.InsertName
(
	@NameText varchar(64)
)
AS
BEGIN
	IF NOT EXISTS (SELECT TOP 1 1 FROM dbo.Name WHERE NameText = @NameText)
	BEGIN
		INSERT INTO dbo.Name
		(
			NameText,
			ModerationFlag
		)
		VALUES
		(
			@NameText,
			0
		)
	END
END
GO

--CREATE TABLE dbo.Person
--(
--	PersonId INT NOT NULL IDENTITY(1,1) PRIMARY KEY,
--	NameText varchar(64) NOT NULL,
--	NameKey varchar(64) NOT NULL
--)
--GO

--CREATE TABLE dbo.ParentChild
--(
--	ParentPersonId INT NOT NULL,
--	ChildPersonId INT NOT NULL
--)
--GO

