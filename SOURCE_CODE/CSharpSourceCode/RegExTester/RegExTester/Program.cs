using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RegExTester
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                System.Console.Out.WriteLine("usage: RegExTester connString regex teststring");
                return;
            }

            string connString = null;
            if (args.Length > 0)
            {
                connString = args[0];
            }
            else
            {
                connString = @"Server=localhost\SQLEXPRESS2014;Initial Catalog=AnalyzeBible_Dev;Trusted_Connection=true";
            }

            string regExText = args[1];
            string testString = args[2];

            foreach (Match m in Regex.Matches(testString, regExText))
            {
                System.Console.Out.WriteLine("[{0}] = {1}", "Name", m.Groups["Name"]);
                foreach (Group g in m.Groups)
                {
                    System.Console.Out.WriteLine("{0}", g.Value);
                }
            }
        }
    }
}
