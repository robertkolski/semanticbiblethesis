-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE dbo.GetParentChild 
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT
		P.NameText as [ParentName],
		C.NameText as [ChildName]
	FROM
		dbo.ParentChildRelationship PCR
		INNER JOIN dbo.Name P
			ON PCR.ParentNameId = P.NameId
		INNER JOIN dbo.Name C
			ON PCR.ChildNameId = C.NameId
	WHERE
		PCR.ModerationFlag = 0
END
GO
