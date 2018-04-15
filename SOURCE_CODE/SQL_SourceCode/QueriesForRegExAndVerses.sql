CREATE PROCEDURE dbo.GetNamedRegExEntry
(
	@RegExCategoryName varchar(64)
)
AS
BEGIN
	DECLARE @RegExCategoryId INT
	SELECT @RegExCategoryId = RegExCategoryId FROM RegExCategory WHERE RegExCategoryName = @RegExCategoryName

	SELECT
		RegExText
	FROM
		RegExEntry
	WHERE
		RegExCategoryId = @RegExCategoryId
END
GO

CREATE PROCEDURE dbo.GetBibleVerses
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
		E.BibleName = @BibleName
END
GO


