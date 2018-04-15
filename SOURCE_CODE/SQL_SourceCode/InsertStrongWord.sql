USE [AnalyzeBible_Dev]
GO

/****** Object:  StoredProcedure [dbo].[InsertStrongWord]    Script Date: 5/2/2016 4:22:08 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[InsertStrongWord]
(
	@StrongNumber varchar(10),
	@StrongWord nvarchar(64),
	@Definition varchar(max)
)
AS
BEGIN
	DECLARE @StrongWordId INT
	SELECT TOP 1 @StrongWordId = StrongWordId FROM dbo.StrongWord WHERE StrongNumber = @StrongNumber

	IF (ISNULL(@StrongWordId, 0) = 0)
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
	ELSE
	BEGIN
		UPDATE dbo.StrongWord
		SET
			StrongWord = @StrongWord,
			[Definition] = @Definition
		WHERE
			StrongWordId = @StrongWordId
	END
END
GO


