using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLFindPeople
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

            HashSet<string> names = null;
            HashSet<string> males = null;
            HashSet<string> females = null;
            HashSet<string> hadVision = null;
            HashSet<string> hadDream = null;
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                names = GetCategory(con, "GetNames", "Name");
                males = GetCategory(con, "GetMales", "Man");
                females = GetCategory(con, "GetFemales", "Woman");
                hadVision = GetCategory(con, "GetVisions", "Person");
                hadDream = GetCategory(con, "GetDreams", "Person");

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

                foreach (string name in names)
                {
                    string gender = null;
                    bool flagHadVision = false;
                    bool flagHadDream = false;
                    if (males.Contains(name))
                    {
                        gender = "Male";
                    }
                    else if (females.Contains(name))
                    {
                        gender = "Female";
                    }
                    if (hadVision.Contains(name))
                    {
                        flagHadVision = true;
                    }
                    if (hadDream.Contains(name))
                    {
                        flagHadDream = true;
                    }

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "dbo.InsertPerson";
                        cmd.Parameters.AddWithValue("@NameText", name);
                        cmd.Parameters.AddWithValue("@Gender", (object)gender ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@HadVision", flagHadVision);
                        cmd.Parameters.AddWithValue("@HadDream", flagHadDream);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static HashSet<string> GetCategory(SqlConnection conn, string category, string matchGroup)
        {
            List<string> regExQueries = new List<string>();
            List<Regex> regExObjects = new List<Regex>();
            HashSet<string> entries = new HashSet<string>();

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = conn;
                cmd.CommandText = "dbo.GetNamedRegExEntry";
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RegExCategoryName", category);
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
                cmd.Connection = conn;
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
                                string entry = match.Groups[matchGroup].Value;
                                entries.Add(entry);
                            }
                        }
                    }
                }
            }
            return entries;
        }
    }
}
