using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SQLBibleToRDFCreator
{
    class Program
    {
        static string connString;
        static string outputFile;

        static void Main(string[] args)
        {
            //if (args.Length < 1)
            //{
            //    System.Console.Out.WriteLine("usage: SQLBibleAddRegEx connString");
            //    return;
            //}

            connString = null;
            if (args.Length > 0)
            {
                connString = args[0];
            }
            else
            {
                connString = @"Server=localhost\SQLEXPRESS2014;Initial Catalog=AnalyzeBible_Dev;Trusted_Connection=true";
            }

            if (args.Length > 1)
            {
                outputFile = args[1];
            }
            else
            {
                //outputFile = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\documents\RDF\bible.rdf");
                outputFile = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\documents\RDF\genesis.rdf");
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(outputFile);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
            }

            XmlNameTable nameTable = new NameTable();
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("person", "http://www.rdfbible.com/dev3/ns/person.owl");
            namespaceManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema#");
            namespaceManager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            namespaceManager.AddNamespace("rdfs", "http://www.w3.org/2000/01/rdf-schema#");

            XmlDocument xmlDocument = new XmlDocument(namespaceManager.NameTable);

            XmlNode root = xmlDocument.CreateElement("rdf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            xmlDocument.AppendChild(root);

            Dictionary<string, HashSet<string>> parentChildren = new Dictionary<string, HashSet<string>>();
            Dictionary<string, Tuple<string, bool, bool>> nameGenderVisionDreams = new Dictionary<string, Tuple<string, bool, bool>>();

            BibleEdition bibleEdition = new BibleEdition();
            bibleEdition.BibleKey = "kjv";
            bibleEdition.BibleName = "KJV";
            bibleEdition.BibleBooks = new List<BibleBook>();

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetBibleVerses";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BibleName", "KJV");
                    cmd.Parameters.AddWithValue("@BibleBook", "Genesis");
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string bookName = (string)reader["BookName"];
                            short chapterNumber = (short)reader["ChapterNumber"];
                            short verseNumber = (short)reader["VerseNumber"];
                            string verseText = (string)reader["VerseText"];

                            BibleBook book = GetBook(bibleEdition, bookName);
                            BibleChapter chapter = GetChapter(book, chapterNumber);
                            BibleVerse verse = GetVerse(chapter, verseNumber);
                            verse.VerseText = verseText;
                            List<string> words = GetWords(verseText).ToList();
                            SetWords(verse, words);
                        }
                    }
                }
            }

            XmlNode xbibleEdition = xmlDocument.CreateElement("bible", "BibleEdition", "http://www.rdfbible.com/dev3/ns/bible.owl");
            root.AppendChild(xbibleEdition);
            XmlAttribute id = xmlDocument.CreateAttribute("rdf", "ID", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            id.Value = bibleEdition.BibleKey;
            xbibleEdition.Attributes.Append(id);
            XmlNode editionName = xmlDocument.CreateElement("bible", "name", "http://www.rdfbible.com/dev3/ns/bible.owl");
            editionName.InnerText = bibleEdition.BibleName;
            xbibleEdition.AppendChild(editionName);

            foreach (var bibleBook in bibleEdition.BibleBooks)
            {
                XmlNode book = xmlDocument.CreateElement("bible", "BibleBook", "http://www.rdfbible.com/dev3/ns/bible.owl");
                root.AppendChild(book);
                XmlAttribute id2 = xmlDocument.CreateAttribute("rdf", "ID", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                id2.Value = bibleBook.BookKey;
                book.Attributes.Append(id2);
                XmlNode bookName = xmlDocument.CreateElement("bible", "name", "http://www.rdfbible.com/dev3/ns/bible.owl");
                bookName.InnerText = bibleBook.BookName;
                book.AppendChild(bookName);
                XmlNode hasBibleBook = xmlDocument.CreateElement("bible", "hasBibleBook", "http://www.rdfbible.com/dev3/ns/bible.owl");
                xbibleEdition.AppendChild(hasBibleBook);
                XmlAttribute hasBibleBookReference = xmlDocument.CreateAttribute("rdf", "resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                hasBibleBookReference.Value = "#" + bibleBook.BookKey;
                hasBibleBook.Attributes.Append(hasBibleBookReference);

                foreach (var bibleChapter in bibleBook.BibleChapters)
                {
                    XmlNode chapter = xmlDocument.CreateElement("bible", "BibleChapter", "http://www.rdfbible.com/dev3/ns/bible.owl");
                    root.AppendChild(chapter);
                    XmlAttribute id3 = xmlDocument.CreateAttribute("rdf", "ID", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                    id3.Value = bibleChapter.ChapterKey;
                    chapter.Attributes.Append(id3);
                    XmlNode chapterNumber = xmlDocument.CreateElement("bible", "number", "http://www.rdfbible.com/dev3/ns/bible.owl");
                    chapterNumber.InnerText = bibleChapter.ChapterNumber.ToString();
                    chapter.AppendChild(chapterNumber);
                    XmlNode hasBibleChapter = xmlDocument.CreateElement("bible", "hasBibleChapter", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                    book.AppendChild(hasBibleChapter);
                    XmlAttribute hasBibleChapterReference = xmlDocument.CreateAttribute("rdf", "resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                    hasBibleChapterReference.Value = "#" + bibleChapter.ChapterKey;
                    hasBibleChapter.Attributes.Append(hasBibleChapterReference);

                    foreach (var bibleVerse in bibleChapter.BibleVerses)
                    {
                        XmlNode verse = xmlDocument.CreateElement("bible", "BibleVerse", "http://www.rdfbible.com/dev3/ns/bible.owl");
                        root.AppendChild(verse);
                        XmlAttribute id4 = xmlDocument.CreateAttribute("rdf", "ID", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                        id4.Value = bibleVerse.VerseKey;
                        verse.Attributes.Append(id4);
                        XmlNode verseNumber = xmlDocument.CreateElement("bible", "number", "http://www.rdfbible.com/dev3/ns/bible.owl");
                        verseNumber.InnerText = bibleVerse.VerseNumber.ToString();
                        verse.AppendChild(verseNumber);
                        XmlNode verseText = xmlDocument.CreateElement("bible", "text", "http://www.rdfbible.com/dev3/ns/bible.owl");
                        verseText.InnerText = bibleVerse.VerseText;
                        verse.AppendChild(verseText);
                        XmlNode hasBibleVerse = xmlDocument.CreateElement("bible", "hasBibleVerse", "http://www.rdfbible.com/dev3/ns/bible.owl");
                        chapter.AppendChild(hasBibleVerse);
                        XmlAttribute hasBibleVerseReference = xmlDocument.CreateAttribute("rdf", "resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                        hasBibleVerseReference.Value = "#" + bibleVerse.VerseKey;
                        hasBibleVerse.Attributes.Append(hasBibleVerseReference);

                        foreach (var bibleWord in bibleVerse.Words)
                        {
                            XmlNode word = xmlDocument.CreateElement("bible", "BibleWord", "http://www.rdfbible.com/dev3/ns/bible.owl");
                            root.AppendChild(word);
                            XmlAttribute id5 = xmlDocument.CreateAttribute("rdf", "ID", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                            id5.Value = bibleWord.WordKey;
                            word.Attributes.Append(id5);
                            XmlElement wordText = xmlDocument.CreateElement("bible", "text", "http://www.rdfbible.com/dev3/ns/bible.owl");
                            wordText.InnerText = bibleWord.Word;
                            word.AppendChild(wordText);
                            XmlNode hasBibleWord = xmlDocument.CreateElement("bible", "hasBibleWord", "http://www.rdfbible.com/dev3/ns/bible.owl");
                            verse.AppendChild(hasBibleWord);
                            XmlAttribute hasBibleWordReference = xmlDocument.CreateAttribute("rdf", "resource", "http://www.rdfbible.com/dev3/ns/bible.owl");
                            hasBibleWordReference.Value = "#" + bibleWord.WordKey;
                            hasBibleWord.Attributes.Append(hasBibleWordReference);
                        }
                    }
                }
            }

            xmlDocument.Save(outputFile);
        }

        public static string GetString(SqlDataReader reader, string name)
        {
            var sqlString = reader.GetSqlString(reader.GetOrdinal(name));
            if (sqlString.IsNull) return null;
            return sqlString.Value;
        }

        public static BibleBook GetBook(BibleEdition edition, string bookName)
        {
            var book = edition.BibleBooks.FirstOrDefault(x => x.BookName == bookName);
            if (book != null)
            {
                return book;
            }
            book = new BibleBook();
            book.BibleChapters = new List<BibleChapter>();
            book.BookKey = edition.BibleKey + "_" + bookName.Replace(' ', '_').ToLowerInvariant();
            book.BookName = bookName;
            edition.BibleBooks.Add(book);
            return book;
        }

        public static BibleChapter GetChapter(BibleBook book, short chapterNumber)
        {
            var chapter = book.BibleChapters.FirstOrDefault(x => x.ChapterNumber == chapterNumber);
            if (chapter != null)
            {
                return chapter;
            }
            chapter = new BibleChapter();
            chapter.BibleVerses = new List<BibleVerse>();
            chapter.ChapterKey = book.BookKey + "_" + chapterNumber;
            chapter.ChapterNumber = chapterNumber;
            book.BibleChapters.Add(chapter);
            return chapter;
        }

        public static BibleVerse GetVerse(BibleChapter chapter, short verseNumber)
        {
            var verse = chapter.BibleVerses.FirstOrDefault(x => x.VerseNumber == verseNumber);
            if (verse != null)
            {
                return verse;
            }
            verse = new BibleVerse();
            verse.Words = new List<BibleWord>();
            verse.VerseKey = chapter.ChapterKey + "_" + verseNumber;
            verse.VerseNumber = verseNumber;
            chapter.BibleVerses.Add(verse);
            return verse;
        }

        public static IEnumerable<string> GetWords(string input)
        {
            Regex regex = new Regex(@"^((?<Word>\w+)($|\W|)*)*");
            foreach (Match m in regex.Matches(input))
            {
                foreach (Capture c in m.Groups["Word"].Captures)
                {
                    yield return c.Value;
                }
            }
        }

        public static void SetWords(BibleVerse verse, List<string> words)
        {
            verse.Words = Lists.RepeatedDefault<BibleWord>(words.Count);

            for (int i = 0; i < words.Count; i++)
            {
                verse.Words[i] = new BibleWord();
                verse.Words[i].Word = words[i];
                verse.Words[i].WordNumber = i + 1;
                verse.Words[i].WordKey = verse.VerseKey + "_" + (i + 1).ToString();
            }
        }
    }
}
