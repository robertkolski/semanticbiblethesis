using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLFindNameInVerse
{
    class Program
    {
        static Regex findRegEx = new Regex(@"^find\s+word\s+(?<Word>\w+)$");
        static Regex addNameInstanceRegEx = new Regex(@"^add\s+name\s+instance\s+(?<Name>\w+)\s+""(?<NameInstance>.*)""$");
        static Regex assignNameInstanceRegEx = new Regex(@"^assign\s+name\s+instance\s+""(?<NameInstance>\w+)""\s+""(?<Book>.*)""\s+(?<Chapter>[0-9]+)\s+(?<Verse>[0-9]+)\s+(?<Word>[0-9])$");
        static string connString;

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

            while (true)
            {
                PrintCommands();
                string command = System.Console.In.ReadLine();
                System.Console.Out.WriteLine();

                string keyword = command.Trim().Split(' ').FirstOrDefault();
                if (keyword == "find")
                {
                    Match m = findRegEx.Match(command.Trim());
                    if (m.Success)
                    {
                        if (m.Groups["Word"].Success)
                        {
                            ListVerses(m.Groups["Word"].Value);
                        }
                        else
                        {
                            System.Console.Out.WriteLine("Could not match for find.");
                        }
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Could not match for find.");
                    }
                }
                else if (keyword == "add")
                {
                    Match m = addNameInstanceRegEx.Match(command.Trim());
                    if (m.Success)
                    {
                        string name = m.Groups["Name"].Value;
                        string nameInstance = m.Groups["NameInstance"].Value;
                        AddNameInstance(name, nameInstance);
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Could not match for moderate.");
                    }
                }
                else if (keyword == "assign")
                {
                    Match m = assignNameInstanceRegEx.Match(command.Trim());
                    if (m.Success)
                    {
                        string nameInstance = m.Groups["NameInstance"].Value;
                        string book = m.Groups["Book"].Value;
                        short chapterNumber = short.Parse(m.Groups["Chapter"].Value);
                        short verseNumber = short.Parse(m.Groups["Verse"].Value);
                        short word = short.Parse(m.Groups["Word"].Value);

                        AssignNameInstance(nameInstance, book, chapterNumber, verseNumber, word);
                    }
                }
                else if (keyword == "quit")
                {
                    break;
                }
                else
                {
                    System.Console.Out.WriteLine("Invalid command.");
                }
            }
        }

        public static void ListVerses(string word)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetBibleVerses";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BibleName", "KJV");
                    cmd.Parameters.AddWithValue("@BibleWord", word);
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        int i = 0;
                        while (sqlDataReader.Read())
                        {
                            string bookName = (string)sqlDataReader["BookName"];
                            short chapterNumber = (short)sqlDataReader["ChapterNumber"];
                            short verseNumber = (short)sqlDataReader["VerseNumber"];
                            string verseText = (string)sqlDataReader["VerseText"];
                            int bibleVerseId = (int)sqlDataReader["BibleVerseId"];
                            short wordNumber = (short)sqlDataReader["WordNumber"];
                            string wordText = (string)sqlDataReader["WordText"];

                            System.Console.WriteLine("{0} {1}:{2} [{3}] \"{4}\" - {5} - {6}",
                                bookName,
                                chapterNumber,
                                verseNumber,
                                wordNumber,
                                wordText,
                                bibleVerseId,
                                verseText);

                            if (i % 10 == 0)
                            {
                                System.Console.Out.WriteLine("Press Enter to continue ...");
                                System.Console.In.ReadLine();
                            }
                        }
                    }
                }
            }
        }

        public static void AddNameInstance(string name, string nameInstance)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.InsertNameInstance";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NameText", name);
                    cmd.Parameters.AddWithValue("@NameInstanceUniqueId", nameInstance);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AssignNameInstance(string nameInstance, string book, short chapterNumber, short verseNumber, short wordNumber)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.InsertNameInstance";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NameInstanceUniqueId", nameInstance);
                    cmd.Parameters.AddWithValue("@BibleName", "KJV");
                    cmd.Parameters.AddWithValue("@BookName", book);
                    cmd.Parameters.AddWithValue("@ChapterNumber", chapterNumber);
                    cmd.Parameters.AddWithValue("@VerseNumber", verseNumber);
                    cmd.Parameters.AddWithValue("@WordNumber", wordNumber);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void PrintCommands()
        {
            System.Console.Out.WriteLine("find word <word>");
            System.Console.Out.WriteLine("add name instance <name> \"<nameInstance>\"");
            System.Console.Out.WriteLine("assign same instance \"<nameInstance>\" \"<book>\" <chapter> <verse> <wordnumber>");
            System.Console.Out.WriteLine("quit - exit program");
        }
    }
}
