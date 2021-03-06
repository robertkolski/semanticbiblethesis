/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [RegExEntryId]
      ,[RegExText]
      ,[RegExCategoryId]
  FROM [AnalyzeBible_Dev].[dbo].[RegExEntry]

SELECT * FROM dbo.RegExCategory

EXEC dbo.InsertRegExEntry
	@RegExCategoryName = 'GetParentChild',
	@RegExText = '(^|\W)[U|u]nto (?<parent>\w+) was born (?<child>\w+)($|\W)'
GO

EXEC dbo.InsertRegExEntry
	@RegExCategoryName = 'GetParentChild',
	@RegExText = '(^|\W)(?<parent>\w+) begat (?<child>\w+)($|\W)'
GO


"($|\W)[U|u]nto (?<parent>\w+) was born (?<child>\w+)(^|\W)" > findParentChildUsingBorn.txt
"($|\W)[W|w]as born (?<child>\w+)(^|\W)" > findBetterBorn.txt
"($|\W)(?<parent>\w+) begat (?<child>\w+)(^|\W)" > findParentBegatChild.txt


SELECT
	*
FROM
	dbo.RegExCategory

SELECT
	*
FROM
	dbo.RegExEntry
WHERE
	RegExCategoryId = 11

UPDATE dbo.RegExEntry
SET
	RegExText = '(^|\W)[U|u]nto (?<Parent>\w+) was born (?<Child>\w+)($|\W)'
WHERE
	RegExEntryId = 28

UPDATE dbo.RegExEntry
SET
	RegExText = '(^|\W)(?<Parent>\w+) begat (?<Child>\w+)($|\W)'
WHERE
	RegExEntryId = 29


EXEC dbo.InsertRegExEntry
	@RegExCategoryName = 'GetNames',
	@RegExText = '(^|\W)[U|u]nto (?<Name>\w+) was born \w+($|\W)'
GO

EXEC dbo.InsertRegExEntry
	@RegExCategoryName = 'GetNames',
	@RegExText = '(^|\W)[U|u]nto \w+ was born (?<Name>\w+)($|\W)'
GO

EXEC dbo.InsertRegExEntry
	@RegExCategoryName = 'GetNames',
	@RegExText = '(^|\W)(?<Name>\w+) begat \w+($|\W)'
GO

EXEC dbo.InsertRegExEntry
	@RegExCategoryName = 'GetNames',
	@RegExText = '(^|\W)\w+ begat (?<Name>\w+)($|\W)'
GO
