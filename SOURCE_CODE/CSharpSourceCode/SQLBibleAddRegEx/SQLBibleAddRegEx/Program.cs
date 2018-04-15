using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLBibleAddRegEx
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                System.Console.Out.WriteLine("usage: SQLBibleAddRegEx connString regexcategory regextext");
                return;
            }

            string connString = args[0];
            string regExCategory = args[1];
            string regExText = args[2];

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.InsertRegExEntry";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@RegExCategoryName", regExCategory);
                    cmd.Parameters.AddWithValue("@RegExText", regExText);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
