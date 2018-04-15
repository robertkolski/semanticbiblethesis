using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SQLImportGreekStrong
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.Out.WriteLine("usage: SQLBibleImporter filename connString");
                return;
            }

            string filename = args[0];
            string connString = args[1];

            XmlDocument xml = new XmlDocument();
            xml.Load(filename);

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                foreach (XmlNode node in xml.SelectNodes(@"//strongsdictionary/entries/entry"))
                {
                    string id = "G" + GetNodeText(node, "strongs");
                    string w = "";
                    XmlNode greek = node.SelectSingleNode("greek");
                    if (greek != null)
                    {
                        if (greek.Attributes["unicode"] != null)
                        {
                            w = greek.Attributes["unicode"].Value;
                        }
                    }
                    string source = GetNodeXml(node, "strongs_derivation");
                    string meaning = GetNodeXml(node, "strongs_def");
                    string kjv_meaning = GetNodeXml(node, "kjv_def");

                    string definition = string.Format("source: {0}\nmeaning: {1}\nkjv meaning: {2}", source, meaning, kjv_meaning);

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = con;
                        cmd.CommandText = "dbo.InsertStrongWord";
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@StrongNumber", id);
                        cmd.Parameters.AddWithValue("@StrongWord", w);
                        cmd.Parameters.AddWithValue("@Definition", definition);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }

        public static string GetNodeText(XmlNode node, string xpath)
        {
            string output = "";
            XmlNode n = node.SelectSingleNode(xpath);
            if (n != null)
            {
                output = n.InnerText;
            }
            return output;
        }

        public static string GetNodeXml(XmlNode node, string xpath)
        {
            string output = "";
            XmlNode n = node.SelectSingleNode(xpath);
            if (n != null)
            {
                output = n.InnerXml;
            }
            return output;
        }
    }
}
