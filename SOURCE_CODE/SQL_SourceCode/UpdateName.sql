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
--	B.BookName = 'Genesis' AND C.ChapterNumber = 1 AND V.VerseNumber = 1
	V.VerseText like '%Sapphira%'

SELECT
	*
FROM
	dbo.Name
WHERE
	NameText IN ('Abel', 'Cain', 'Ananias', 'Sapphira', 'Rhoda', 'Priscilla', 'Aquila')

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

UPDATE dbo.RegExEntry
SET
	RegExText = '($|\W)[N|n]amed (?<Name>\w+)(^|\W)'
WHERE
	RegExEntryId = 23



--And as Peter knocked at the door of the gate, a damsel came to hearken, named Rhoda.


SELECT
	*
FROM
	dbo.BibleVerse
WHERE
	VerseText like '%Priscilla%'

SELECT
	*
FROM
	dbo.RegExEntry

INSERT INTO dbo.RegExEntry
(
	RegExText,
	RegExCategoryId
)
VALUES
(
	'($|\W)(?<Name>\w+) his wife(^|\W)',
	10
)

INSERT INTO dbo.RegExEntry
(
	RegExText,
	RegExCategoryId
)
VALUES
(
	'($|\W)[W|w]ith his wife (?<Name>\w+)(^|\W)',
	10
)

--with his wife Priscilla


--And found a certain Jew named Aquila, born in Pontus, lately come from Italy, with his wife Priscilla; (because that Claudius had commanded all Jews to depart from Rome:) and came unto them.


UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'Blank name'
WHERE
	NameText = ''

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'Blank name'
WHERE
	NameText = ''

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'a is not a name'
WHERE NameText = 'a'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'and is a conjunction'
WHERE NameText = 'and'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'sons is a pronoun'
WHERE NameText = 'sons'


UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'he is a pronoun'
WHERE NameText = 'he'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'three is a number'
WHERE NameText = 'three'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'of is not a name'
WHERE NameText = 'of'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'in is not a name'
WHERE NameText = 'in'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'they is a pronoun'
WHERE NameText = 'they'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'them is a pronoun'
WHERE NameText = 'them'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'thee is a pronoun'
WHERE NameText = 'thee'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'that is not a name'
WHERE NameText = 'that'


UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'his is a possessive pronoun'
WHERE NameText = 'his'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'children is not a name'
WHERE NameText = 'children'


UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'who is not a name'
WHERE NameText = 'who'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'the is not a name'
WHERE NameText = 'the'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'which is not a name'
WHERE NameText = 'which'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'more is not a name'
WHERE NameText = 'more'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'twenty is a number'
WHERE NameText = 'twenty'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'also is not a name'
WHERE NameText = 'also'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'me is a pronoun'
WHERE NameText = 'me'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'her is a possessive pronoun'
WHERE NameText = 'her'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'strange is not a name'
WHERE NameText = 'strange'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'him is a pronoun'
WHERE NameText = 'him'

UPDATE dbo.Name
SET ModerationFlag = 1,
ModerationReason = 'king is a title'
WHERE NameText = 'king'

SELECT
	*
FROM
	dbo.Name
WHERE
	NameText = ''