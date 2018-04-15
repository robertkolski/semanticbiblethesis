SELECT * FROM dbo.Name WHERE NameText = 'Adam'
SELECT * FROM dbo.Name WHERE NameText = 'Eve'
SELECT * FROM dbo.Name WHERE NameText = 'Cain'
SELECT * FROM dbo.Name WHERE NameText = 'Abel'
SELECT * FROM dbo.Name WHERE NameText = 'Seth'
SELECT * FROM dbo.Name WHERE NameText = ''

SELECT * FROM dbo.RegExEntry

SELECT * FROM dbo.Person WHERE NameId = 258

--EXEC dbo.InsertRegExEntry 'GetNames', '(^|\W)bare\W(his|her)\W(brother|sister)\W(?<Name>\w+)($|\W)'
--EXEC dbo.InsertRegExEntry 'GetNames', '(^|\W)talked\s+with\s+(?<Name>\w+)\s+his\s+brother($|\W)'

SELECT * FROM dbo.RegExCategory


SELECT * FROM dbo.Name

SELECT 
	P.NameText [ParentName],
	C.NameText [ChildName],
	PCR.*
FROM
	dbo.ParentChildRelationship PCR
	INNER JOIN dbo.Name P
		ON
			P.NameId = PCR.ParentNameId
	INNER JOIN dbo.Name C
		ON
			C.NameId = PCR.ChildNameId
WHERE
	C.NameText = 'Abel'

SELECT
	*
FROM
	