CREATE TABLE dbo.Person
(
	PersonId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	NameId INT CONSTRAINT FK_NameId FOREIGN KEY REFERENCES dbo.Name(NameId),
	Gender varchar(10),
	HadVision bit,
	HadDream bit
)
GO

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
CREATE PROCEDURE dbo.InsertPerson
	@NameText varchar(64),
	@Gender varchar(10),
	@HadVision bit,
	@HadDream bit
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NameId INT
	SELECT TOP 1 @NameId = NameId FROM dbo.Name WHERE NameText = @NameText
	
	INSERT INTO dbo.Person  
	(
		NameId,
		Gender,
		HadVision,
		HadDream
	)
	VALUES
	(
		@NameId,
		@Gender,
		@HadVision,
		@HadDream
	)
END
GO
