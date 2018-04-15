using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SQLBibleImporter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.Out.WriteLine("usage: SQLBibleImporter filename edition connString");
                return;
            }

            string filename = args[0];
            string edition = args[1];
            string connString = args[2];

            XmlDocument xml = new XmlDocument();
            xml.Load(filename);

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                foreach (XmlNode node in xml.SelectNodes(@"//BIBLEBOOK/CHAPTER/VERS"))
                {
                    //System.Console.Out.WriteLine("Book {0} Chapter {1} Verse {2} Text {3}",
                    //    node.ParentNode.ParentNode.Attributes["bname"].Value,
                    //    node.ParentNode.Attributes["cnumber"].Value,
                    //    node.Attributes["vnumber"].Value,
                    //    node.InnerText
                    //);

                    short bookNumber = short.Parse(node.ParentNode.ParentNode.Attributes["bnumber"].Value);
                    if (bookNumber < 54)
                    {
                        continue;
                    }

                    string bookName = node.ParentNode.ParentNode.Attributes["bname"].Value;
                    short chapterNumber = short.Parse(node.ParentNode.Attributes["cnumber"].Value);
                    short verseNumber = short.Parse(node.Attributes["vnumber"].Value);
                    string verseText = node.InnerText;

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "dbo.InsertBibleVerse";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@BibleName", edition);
                        cmd.Parameters.AddWithValue("@BookNumber", bookNumber);
                        cmd.Parameters.AddWithValue("@BookName", bookName);
                        cmd.Parameters.AddWithValue("@ChapterNumber", chapterNumber);
                        cmd.Parameters.AddWithValue("@VerseNumber", verseNumber);
                        cmd.Parameters.AddWithValue("@VerseText", verseText);
                        cmd.ExecuteNonQuery();
                    }

                    List<string> words = GetWords(verseText).ToList();
                    for (int i = 0; i < words.Count; i++)
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.Connection = con;
                            cmd.CommandText = "dbo.InsertBibleWord";
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@BibleName", edition);
                            cmd.Parameters.AddWithValue("@BookNumber", bookNumber);
                            cmd.Parameters.AddWithValue("@BookName", bookName);
                            cmd.Parameters.AddWithValue("@ChapterNumber", chapterNumber);
                            cmd.Parameters.AddWithValue("@VerseNumber", verseNumber);
                            cmd.Parameters.AddWithValue("@WordNumber", (short)(i + 1));
                            cmd.Parameters.AddWithValue("@WordText", words[i]);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
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
    }
}
