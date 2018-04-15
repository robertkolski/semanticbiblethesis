using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLFindParentChild
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

            string regExCategory = "GetParentChild";

            List<string> regExQueries = new List<string>();
            List<Regex> regExObjects = new List<Regex>();
            HashSet<string> names = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
            List<Tuple<string, string, int>> parentChildList = new List<Tuple<string, string, int>>();
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
                    cmd.CommandText = "dbo.GetName";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ModerationFlag", false);
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            names.Add((string)sqlDataReader["NameText"]);
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
                            int verseId = (int)sqlDataReader["BibleVerseId"];

                            foreach (var regExObject in regExObjects)
                            {
                                var matches = regExObject.Matches(text);
                                foreach (Match match in matches)
                                {
                                    string parent = match.Groups["Parent"].Value;
                                    string child = match.Groups["Child"].Value;
                                    parentChildList.Add(Tuple.Create(parent, child, verseId));
                                }
                            }
                        }
                    }
                }

                foreach (var parentChild in parentChildList)
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        bool moderationFlag = false;
                        string moderationReason = null;
                        if (!names.Contains(parentChild.Item1))
                        {
                            moderationFlag = true;
                            moderationReason = "parent is not a name";
                        }
                        else if (!names.Contains(parentChild.Item2))
                        {
                            moderationFlag = true;
                            moderationReason = "child is not a name";
                        }

                        cmd.Connection = con;
                        cmd.CommandText = "dbo.InsertParentChild";
                        cmd.Parameters.AddWithValue("@ParentName", parentChild.Item1);
                        cmd.Parameters.AddWithValue("@ChildName", parentChild.Item2);
                        cmd.Parameters.AddWithValue("@BibleVerseId", parentChild.Item3);
                        cmd.Parameters.AddWithValue("@ModerationFlag", moderationFlag);
                        cmd.Parameters.AddWithValue("@ModerationReason", (object)moderationReason ?? DBNull.Value);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
