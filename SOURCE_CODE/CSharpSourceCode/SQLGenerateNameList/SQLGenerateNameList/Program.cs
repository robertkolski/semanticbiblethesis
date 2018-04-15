using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLGenerateNameList
{
    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length < 1)
            //{
            //    System.Console.Out.WriteLine("usage: SQLBibleAddRegEx connString");
            //    return;
            //}

            string connString = null;
            if (args.Length > 0)
            {
                connString = args[0];
            }
            else
            {
                connString = @"Server=localhost\SQLEXPRESS2014;Initial Catalog=AnalyzeBible_Dev;Trusted_Connection=true";
            }

            string regExCategory = "GetNames";

            List<string> regExQueries = new List<string>();
            List<Regex> regExObjects = new List<Regex>();
            HashSet<string> names = new HashSet<string>();
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetNamedRegExEntry";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RegExCategoryName", regExCategory);
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            string regExText = (string)sqlDataReader["RegExText"];
                            regExQueries.Add(regExText);
                            regExObjects.Add(new Regex(regExText, RegexOptions.Compiled));
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetBibleVerses";
                    cmd.Parameters.AddWithValue("@BibleName", "KJV");
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            string book = (string)sqlDataReader["BookName"];
                            short chapter = (short)sqlDataReader["ChapterNumber"];
                            short verse = (short)sqlDataReader["VerseNumber"];
                            string text = (string)sqlDataReader["VerseText"];

                            foreach (var regExObject in regExObjects)
                            {
                                var matches = regExObject.Matches(text);
                                foreach (Match match in matches)
                                {
                                    string name = match.Groups["Name"].Value;
                                    names.Add(name);
                                }
                            }
                        }
                    }
                }

                if (names.Contains("Adam"))
                {
                    System.Console.Out.WriteLine("Adam was found");
                }
                if (names.Contains("Rhoda"))
                {
                    System.Console.Out.WriteLine("Rhoda was found");
                }

                foreach (string name in names)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "dbo.InsertName";
                        cmd.Parameters.AddWithValue("@NameText", name);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
