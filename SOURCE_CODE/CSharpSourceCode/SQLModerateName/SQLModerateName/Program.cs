using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLModerateName
{
    class Program
    {
        static Regex listRegEx = new Regex(@"^list\s+(?<NameId>[0-9]+)$");
        static Regex listRangeRegEx = new Regex(@"^list\s+(?<MinNameId>[0-9]+)\s*\-\s*(?<MaxNameId>[0-9]+)$");
        static Regex moderateRegEx = new Regex(@"^moderate\s+(?<NameId>[0-9]+)\s+(?<Flag>[0|1])\s+(?<Reason>.*)$");
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
                if (keyword == "list")
                {
                    Match m = listRegEx.Match(command.Trim());
                    Match m2 = listRangeRegEx.Match(command.Trim());
                    if (m.Success)
                    {
                        if (m.Groups["NameId"].Success)
                        {
                            int nameId = int.Parse(m.Groups["NameId"].Value);
                            List(nameId);
                        }
                        else
                        {
                            System.Console.Out.WriteLine("Could not match for list.");
                        }
                    }
                    else if (m2.Success)
                    {
                        if (m2.Groups["MinNameId"].Success && m2.Groups["MaxNameId"].Success)
                        {
                            int minNameId = int.Parse(m2.Groups["MinNameId"].Value);
                            int maxNameId = int.Parse(m2.Groups["MaxNameId"].Value);
                            List(minNameId, maxNameId);
                        }
                        else
                        {
                            System.Console.Out.WriteLine("Could not match for list.");
                        }
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Could not match for list.");
                    }
                }
                else if (keyword == "moderate")
                {
                    Match m = moderateRegEx.Match(command.Trim());
                    if (m.Success)
                    {
                        foreach (Group g in m.Groups)
                        {
                            System.Console.Out.WriteLine(g.Value);
                        }

                        int nameId = int.Parse(m.Groups["NameId"].Value);
                        bool flag = int.Parse(m.Groups["Flag"].Value) == 1;
                        string reason = m.Groups["Reason"].Value;
                        if (reason.Trim().ToLower() == "null")
                        {
                            reason = null;
                        }
                        Moderate(nameId, flag, reason);
                    }
                    else
                    {
                        System.Console.Out.WriteLine("Could not match for moderate.");
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

        public static void List(int nameId)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetName";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NameId", nameId);
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            int nameIdR = (int)sqlDataReader["NameId"];
                            string nameText = (string)sqlDataReader["NameText"];
                            bool flag = (bool)sqlDataReader["ModerationFlag"];
                            string reason = (string)sqlDataReader["ModerationReason"];

                            System.Console.WriteLine("{0:##########} {1} {2} {3}",
                                nameIdR,
                                nameText.FormatLength(20, ' '),
                                flag ? 1 : 0,
                                reason.FormatLength(20, ' '));
                        }
                    }
                }
            }
        }

        public static void List(int minNameId, int maxNameId)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetName";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@MinNameId", minNameId);
                    cmd.Parameters.AddWithValue("@MaxNameId", maxNameId);
                    using (SqlDataReader sqlDataReader = cmd.ExecuteReader())
                    {
                        while (sqlDataReader.Read())
                        {
                            int nameIdR = (int)sqlDataReader["NameId"];
                            string nameText = (string)sqlDataReader["NameText"];
                            bool flag = (bool)sqlDataReader["ModerationFlag"];
                            string reason = sqlDataReader.GetString("ModerationReason");

                            System.Console.WriteLine("{0:##########} {1} {2} {3}",
                                nameIdR,
                                nameText.FormatLength(20, ' '),
                                flag ? 1 : 0,
                                reason.FormatLength(20, ' '));
                        }
                    }
                }
            }
        }

        public static void Moderate(int nameId, bool flag, string reason)
        {
            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.UpdateName";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@NameId", nameId);
                    cmd.Parameters.AddWithValue("@ModerationFlag", flag);
                    cmd.Parameters.AddWithValue("@ModerationReason", reason);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void PrintCommands()
        {
            System.Console.Out.WriteLine("list nameId | minNameId - maxNameId");
            System.Console.Out.WriteLine("moderate nameId 0|1 why");
            System.Console.Out.WriteLine("quit - exit program");
        }
    }
}
