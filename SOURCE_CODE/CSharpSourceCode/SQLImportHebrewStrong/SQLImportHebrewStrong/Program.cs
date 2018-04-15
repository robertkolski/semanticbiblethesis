using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SQLImportHebrewStrong
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.Out.WriteLine("usage: SQLImportHebrewStrong filename connString");
                return;
            }

            string filename = args[0];
            string connString = args[1];

            XmlDocument xml = new XmlDocument();
            xml.Load(filename);

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();
                XmlNode lexicon = xml.ChildNodes[1];
                //foreach (XmlNode node in xml.SelectNodes(@"//lexicon/entry"))
                foreach (XmlNode node in lexicon.ChildNodes)
                {
                    string id = node.Attributes["id"].Value;
                    string w = GetNodeText(node, "w");
                    string source = GetNodeXml(node, "source");
                    string meaning = GetNodeXml(node, "meaning");
                    string usage = GetNodeXml(node, "usage");

                    string definition = string.Format("source: {0}\nmeaning: {1}\nusage: {2}", source, meaning, usage);

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
            XmlNode n = GetChildNodeByName(node, xpath);
            if (n != null)
            {
                output = n.InnerText;
            }
            return output;
        }

        public static string GetNodeXml(XmlNode node, string xpath)
        {
            string output = "";
            XmlNode n = GetChildNodeByName(node, xpath);
            if (n != null)
            {
                output = n.InnerXml;
            }
            return output;
        }

        public static XmlNode GetChildNodeByName(XmlNode node ,string name)
        {
            foreach (XmlNode child in node.ChildNodes)
            {
                if (child.Name == name)
                {
                    return child;
                }
            }
            return null;
        }
    }
}
