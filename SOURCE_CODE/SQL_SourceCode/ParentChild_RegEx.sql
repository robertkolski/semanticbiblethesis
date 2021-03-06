
SELECT * FROM dbo.RegExCategory
SELECT * FROM dbo.RegExEntry

EXEC dbo.GetNamedRegExEntry 'GetParentChild'


EXEC dbo.InsertRegExEntry 'GetParentChild', '(^|\W)(?<Parent>\w+) knew (\s|\S)*bare (?<Child>\w+)($|\W)'
EXEC dbo.InsertRegExEntry 'GetParentChild', '(^|\W)\w+ knew (?<Parent>\w+) (\s|\S)*bare (?<Child>\w+)($|\W)'
EXEC dbo.InsertRegExEntry 'GetParentChild', '(^|\W)(?<Parent>\w+)\W(\s|\S)*\Wbare ((his|her) (brother|sister))? (?<Child>\w+)($|\W)'
EXEC dbo.InsertRegExEntry 'GetParentChild', '(^|\W)(?<Parent>\w+), of whom was born (?<Child>\w+)($|\W)'

EXEC dbo.InsertRegExEntry 'GetNames', '(^|\W)(?<Name>\w+), of whom was born \w+($|\W)'
EXEC dbo.InsertRegExEntry 'GetNames', '(^|\W)\w+, of whom was born (?<Name>\w+)($|\W)'
SELECT * FROM dbo.Name