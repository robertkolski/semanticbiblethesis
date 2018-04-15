EXEC dbo.InsertRegExEntry 'GetMales', '(^|\W)(?<Man>\w+) begat($|\W)'
EXEC dbo.InsertRegExEntry 'GetFemales', '(^|\W)(?<Woman>\w+) bare($|\W)'
EXEC dbo.InsertRegExEntry 'GetVisions', '(^|\W)(?<Person>\w+) had a vision($|\W)'
EXEC dbo.InsertRegExEntry 'GetDreams', '(^|\W)(?<Person>\w+) dreamed($|\W)'



