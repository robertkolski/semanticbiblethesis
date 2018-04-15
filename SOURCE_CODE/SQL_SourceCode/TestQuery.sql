SELECT
	E.BibleName,
	B.BookName,
	B.BookNumber,
	C.ChapterNumber,
	V.VerseNumber,
	V.VerseText
FROM
	BibleVerse V
	INNER JOIN BibleChapter C
		ON V.BibleChapterId = C.BibleChapterId
	INNER JOIN BibleBook B
		ON C.BibleBookId = B.BibleBookId
	INNER JOIN BibleEdition E
		ON B.BibleEditionId = E.BibleEditionId
WHERE
--B.BookName = 'Acts' AND C.ChapterNumber = 8 -- AND V.VerseNumber = 1
V.BibleVerseId = 29806

	--B.BookName = '1 Timothy' AND C.ChapterNumber = 3 -- AND V.VerseNumber = 1


SELECT
	E.BibleName,
	B.BookName,
	C.ChapterNumber,
	V.VerseNumber,
	V.VerseText
FROM
	BibleVerse V
	INNER JOIN BibleChapter C
		ON V.BibleChapterId = C.BibleChapterId
	INNER JOIN BibleBook B
		ON C.BibleBookId = B.BibleBookId
	INNER JOIN BibleEdition E
		ON B.BibleEditionId = E.BibleEditionId
WHERE
	B.BookName like '%1 peter%' AND C.ChapterNumber = 3 --AND V.VerseNumber = 1
--	V.VerseText like '%Rhoda%'
	--V.VerseText like '%taking thought%'
--and V.VerseText like '%harlot%'





SELECT
	*
FROM
	dbo.Name
WHERE
	NameText IN ('Abel', 'Cain', 'Ananias', 'Sapphira', 'Rhoda', 'Priscilla', 'Aquilla')

SELECT
	*
FROM
	dbo.BibleVerse
WHERE
	VerseText like '%Abel%' --and VerseText like '%the son of%'
	and VerseText like '%brother%'

SELECT
	*
FROM
	dbo.BibleVerse
WHERE
	VerseText like '%Rhoda%'

SELECT
	*
FROM
	dbo.RegExEntry

--UPDATE dbo.RegExEntry
--SET
--	RegExText = '($|\W)[N|n]amed (?<Name>\w+)(^|\W)'
--WHERE
--	RegExEntryId = 23



--And as Peter knocked at the door of the gate, a damsel came to hearken, named Rhoda.


SELECT
	*
FROM
	dbo.BibleVerse
WHERE
	VerseText like '%Sapphira%'