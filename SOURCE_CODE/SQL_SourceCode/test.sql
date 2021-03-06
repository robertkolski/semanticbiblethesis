/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 [BibleBookId]
      ,[BookNumber]
      ,[BookName]
      ,[BibleEditionId]
  FROM [AnalyzeBible_Dev].[dbo].[BibleBook]

SELECT
	B.BookName, C.ChapterNumber, V.VerseNumber, V.VerseText
FROM
	BibleVerse V
	INNER JOIN BibleChapter C
		ON V.BibleChapterId = C.BibleChapterId
	INNER JOIN BibleBook B
		ON C.BibleBookId = B.BibleBookId
WHERE
	V.VerseText LIKE '%Cain%'