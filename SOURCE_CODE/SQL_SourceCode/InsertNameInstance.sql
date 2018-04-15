CREATE PROCEDURE dbo.InsertNameInstance
(
	@NameText varchar(64),
	@NameInstanceUniqueId varchar(100)
)
AS
BEGIN
	DECLARE @NameId INT
	SELECT TOP 1 @NameId = NameId FROM dbo.Name WHERE NameText = @NameText

	IF EXISTS (SELECT TOP 1 1 FROM dbo.NameInstance WHERE NameInstanceUnqiueId = @NameInstanceUniqueId)
	BEGIN
		RETURN
	END

	INSERT INTO dbo.NameInstance
	(
		NameId,
		NameInstanceUnqiueId
	)
	VALUES
	(
		@NameId,
		@NameInstanceUniqueId
	)
END
GO