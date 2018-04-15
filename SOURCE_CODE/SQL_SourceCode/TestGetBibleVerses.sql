USE [AnalyzeBible_Dev]
GO

/****** Object:  StoredProcedure [dbo].[GetBibleVerses]    Script Date: 5/2/2016 7:34:33 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



ALTER PROCEDURE [dbo].[GetBibleVerses]
(
	@BibleName varchar(64) = null,
	@BibleWord varchar(64) = null
)
AS
BEGIN
	IF (@BibleWord IS NULL)
	BEGIN
		SELECT
			E.BibleName,
			B.BookName,
			C.ChapterNumber,
			V.VerseNumber,
			V.VerseText,
			V.BibleVerseId
		FROM
			BibleVerse V
			INNER JOIN BibleChapter C
				ON V.BibleChapterId = C.BibleChapterId
			INNER JOIN BibleBook B
				ON C.BibleBookId = B.BibleBookId
			INNER JOIN BibleEdition E
				ON B.BibleEditionId = E.BibleEditionId
		WHERE
			E.BibleName = @BibleName
	END
	ELSE
	BEGIN
		SELECT DISTINCT
			E.BibleName,
			B.BookName,
			C.ChapterNumber,
			V.VerseNumber,
			V.VerseText,
			V.BibleVerseId,
			W.WordNumber,
			W.WordText
		FROM
			BibleVerse V
			INNER JOIN BibleChapter C
				ON V.BibleChapterId = C.BibleChapterId
			INNER JOIN BibleBook B
				ON C.BibleBookId = B.BibleBookId
			INNER JOIN BibleEdition E
				ON B.BibleEditionId = E.BibleEditionId
			INNER JOIN BibleWord W
				ON V.BibleVerseId = W.BibleVerseId
		WHERE
			E.BibleName = @BibleName
		AND W.WordText = @BibleWord
	END
END


GO


		SELECT DISTINCT
			E.BibleName,
			B.BookName,
			C.ChapterNumber,
			V.VerseNumber,
			V.VerseText,
			V.BibleVerseId--,
			--W.WordNumber,
			--W.WordText
		FROM
			BibleVerse V
			INNER JOIN BibleChapter C
				ON V.BibleChapterId = C.BibleChapterId
			INNER JOIN BibleBook B
				ON C.BibleBookId = B.BibleBookId
			INNER JOIN BibleEdition E
				ON B.BibleEditionId = E.BibleEditionId
			--INNER JOIN BibleWord W
			--	ON V.BibleVerseId = W.BibleVerseId
		WHERE
			E.BibleName = 'KJV'
		--AND W.WordText like '%Adam%'
		AND V.VerseText like '%Adam%'
	
SELECT
	*
FROM
	BibleWord
WHERE
	BibleVerseId = 50