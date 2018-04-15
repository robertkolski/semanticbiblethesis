CREATE PROCEDURE dbo.GetName
(
	@NameId int = null,
	@MinNameId int = null,
	@MaxNameId int = null,
	@ModerationFlag bit = null
)
AS
BEGIN
	IF (@NameId IS NOT NULL)
	BEGIN
		SELECT
			*
		FROM
			dbo.Name
		WHERE
			NameId = @NameId		
	END
	ELSE IF (@MinNameId IS NOT NULL AND @MaxNameId IS NOT NULL)
	BEGIN
		SELECT
			*
		FROM
			dbo.Name
		WHERE
			NameId >= @MinNameId
		AND NameId <= @MaxNameId
	END
	ELSE
	BEGIN
		SELECT
			*
		FROM
			dbo.Name
		WHERE
			@ModerationFlag IS NULL
		OR	(@ModerationFlag IS NOT NULL AND ModerationFlag = @ModerationFlag)
	END
END
GO

CREATE PROCEDURE dbo.UpdateName
(
	@NameId int,
	@ModerationFlag bit,
	@ModerationReason varchar(MAX)
)
AS
BEGIN
	UPDATE dbo.Name
	SET
		ModerationFlag = @ModerationFlag,
		ModerationReason = @ModerationReason
	WHERE
		NameId = @NameId
END
GO