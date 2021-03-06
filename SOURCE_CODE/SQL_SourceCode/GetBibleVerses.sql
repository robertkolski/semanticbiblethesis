USE [AnalyzeBible_Dev]
GO
/****** Object:  StoredProcedure [dbo].[GetBibleVerses]    Script Date: 4/22/2016 1:04:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[GetBibleVerses]
(
	@BibleName varchar(64) = null
)
AS
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
