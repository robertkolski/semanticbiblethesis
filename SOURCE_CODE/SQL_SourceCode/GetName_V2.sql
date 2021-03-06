USE [AnalyzeBible_Dev]
GO
/****** Object:  StoredProcedure [dbo].[GetName]    Script Date: 4/29/2016 1:10:03 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE [dbo].[GetName]
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
			N.*, P.Gender, P.HadVision, P.HadDream
		FROM
			dbo.Name N
			LEFT JOIN dbo.Person P
				ON P.NameId = N.NameId
		WHERE
			N.NameId = @NameId		
	END
	ELSE IF (@MinNameId IS NOT NULL AND @MaxNameId IS NOT NULL)
	BEGIN
		SELECT
			N.*, P.Gender, P.HadVision, P.HadDream
		FROM
			dbo.Name N
			LEFT JOIN dbo.Person P
				ON P.NameId = N.NameId
		WHERE
			N.NameId >= @MinNameId
		AND N.NameId <= @MaxNameId
	END
	ELSE
	BEGIN
		SELECT
			N.*, P.Gender, P.HadVision, P.HadDream
		FROM
			dbo.Name N
			LEFT JOIN dbo.Person P
				ON P.NameId = N.NameId
		WHERE
			@ModerationFlag IS NULL
		OR	(@ModerationFlag IS NOT NULL AND N.ModerationFlag = @ModerationFlag)
	END
END
