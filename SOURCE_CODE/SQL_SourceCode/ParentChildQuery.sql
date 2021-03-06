/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [ParentChildRelationshipId]
      ,[ParentNameId]
      ,[ChildNameId]
      ,[BibleVerseId]
      ,[ModerationFlag]
      ,[ModerationReason]
  FROM [AnalyzeBible_Dev].[dbo].[ParentChildRelationship]

SELECT
	ParentName = P.NameText,
	ChildName = C.NameText,
	Verse = V.VerseText,
	Invalid = PCR.ModerationFlag
FROM
	ParentChildRelationship PCR
	INNER JOIN Name P
		ON PCR.ParentNameId = P.NameId
	INNER JOIN Name C
		ON PCR.ChildNameId = C.NameId
	INNER JOIN BibleVerse V
		ON PCR.BibleVerseId = V.BibleVerseId
WHERE
	PCR.ModerationFlag = 0
--AND C.NameText = 'Jesus'

--TRUNCATE TABLE [dbo].[ParentChildRelationship]