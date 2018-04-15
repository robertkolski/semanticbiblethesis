CREATE TABLE dbo.VisionDream
(
	VisionDreamId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	PersonId INT NOT NULL CONSTRAINT FK_Person FOREIGN KEY REFERENCES dbo.Person(PersonId),
	IsVision bit NOT NULL,
	InterpretedByPersonId INT CONSTRAINT FK_InterpretedBy FOREIGN KEY REFERENCES dbo.Person(PersonId),
	VisionDreamDescription varchar(max)
)
GO

CREATE PROCEDURE dbo.InsertVisionDream
(
	@PersonName varchar(64),
	@IsVision bit,
	@InterpretedByName varchar(64),
	@VisionDreamDescription varchar(max)
)
AS
BEGIN
	DECLARE @PersonId INT
	DECLARE @InterpretedByPersonId INT

	SELECT TOP 1
		@PersonId = PersonId
	FROM
		dbo.Person P
		INNER JOIN dbo.Name N
			ON P.NameId = N.NameId
	WHERE
		N.NameText = @PersonName

	IF (@PersonId IS NULL)
	BEGIN
		DECLARE @ErrorText varchar(2048)
		SET @ErrorText = 'The person with the name' + ISNULL(@PersonName, '<NULL>') + ' was not found'
		RAISERROR (@ErrorText, 16, 1)
	END

	SELECT TOP 1
		@InterpretedByPersonId = PersonId
	FROM
		dbo.Person P
		INNER JOIN dbo.Name N
			ON P.NameId = N.NameId
	WHERE
		N.NameText = @InterpretedByName

	INSERT INTO dbo.VisionDream
	(
		PersonId,
		IsVision,
		InterpretedByPersonId,
		VisionDreamDescription
	)
	VALUES
	(
		@PersonId,
		@IsVision,
		@InterpretedByPersonId,
		@VisionDreamDescription
	)
END
GO


