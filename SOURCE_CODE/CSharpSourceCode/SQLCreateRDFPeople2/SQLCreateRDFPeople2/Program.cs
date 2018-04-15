using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SQLCreateRDFPeople2
{
    class Program
    {
        static string connString;
        static string outputFile;

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

            if (args.Length > 1)
            {
                outputFile = args[1];
            }
            else
            {
                outputFile = Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\documents\RDF\default2.rdf");
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(outputFile);
                if (!fileInfo.Directory.Exists)
                {
                    fileInfo.Directory.Create();
                }
            }

            XmlNameTable nameTable = new NameTable();
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(nameTable);
            namespaceManager.AddNamespace("person", "http://www.rdfbible.com/dev2/ns/person.owl");
            namespaceManager.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema#");
            namespaceManager.AddNamespace("rdf", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            namespaceManager.AddNamespace("rdfs", "http://www.w3.org/2000/01/rdf-schema#");

            XmlDocument xmlDocument = new XmlDocument(namespaceManager.NameTable);

            XmlNode root = xmlDocument.CreateElement("rdf", "RDF", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
            xmlDocument.AppendChild(root);

            Dictionary<string, HashSet<string>> parentChildren = new Dictionary<string, HashSet<string>>();
            Dictionary<string, Tuple<string, bool, bool>> nameGenderVisionDreams = new Dictionary<string, Tuple<string, bool, bool>>();

            using (SqlConnection con = new SqlConnection(connString))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetParentChild";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string parentName = (string)reader["ParentName"];
                            string childName = (string)reader["ChildName"];
                            if (!parentChildren.ContainsKey(parentName))
                            {
                                parentChildren.Add(parentName, new HashSet<string>());
                            }
                            parentChildren[parentName].Add(childName);
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = con;
                    cmd.CommandText = "dbo.GetName";
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ModerationFlag", false);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string nameText = (string)reader["NameText"];
                            string gender = GetString(reader, "Gender");
                            bool flagHadVision = (bool)reader["HadVision"];
                            bool flagHadDream = (bool)reader["HadDream"];

                            if (!nameGenderVisionDreams.ContainsKey(nameText))
                            {
                                nameGenderVisionDreams.Add(nameText, Tuple.Create(gender, flagHadVision, flagHadDream));
                            }
                        }
                    }
                }
            }

            foreach (var nameGenderVisionDream in nameGenderVisionDreams)
            {
                XmlNode person = xmlDocument.CreateElement("person", "Person", "http://www.rdfbible.com/dev2/ns/person.owl");
                root.AppendChild(person);
                XmlAttribute id = xmlDocument.CreateAttribute("rdf", "ID", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                id.Value = nameGenderVisionDream.Key.ToLower();
                person.Attributes.Append(id);
                XmlNode personName = xmlDocument.CreateElement("person", "name", "http://www.rdfbible.com/dev2/ns/person.owl");
                personName.InnerText = nameGenderVisionDream.Key;
                person.AppendChild(personName);
                if (parentChildren.ContainsKey(nameGenderVisionDream.Key))
                {
                    HashSet<string> children = parentChildren[nameGenderVisionDream.Key];
                    foreach (string child in children)
                    {
                        XmlNode hasChildNode = xmlDocument.CreateElement("person", "hasChild", "http://www.rdfbible.com/dev2/ns/person.owl");
                        person.AppendChild(hasChildNode);
                        XmlAttribute hasChildReference = xmlDocument.CreateAttribute("rdf", "resource", "http://www.w3.org/1999/02/22-rdf-syntax-ns#");
                        hasChildReference.Value = "#" + child.ToLower();
                        hasChildNode.Attributes.Append(hasChildReference);
                    }
                }
                if (nameGenderVisionDream.Value.Item1 != null)
                {
                    XmlNode genderNode = xmlDocument.CreateElement("person", "gender", "http://www.rdfbible.com/dev2/ns/person.owl");
                    person.AppendChild(genderNode);
                    genderNode.InnerText = nameGenderVisionDream.Value.Item1;
                }
                if (nameGenderVisionDream.Value.Item2)
                {
                    XmlNode visionNode = xmlDocument.CreateElement("person", "hadVision", "http://www.rdfbible.com/dev2/ns/person.owl");
                    person.AppendChild(visionNode);
                    visionNode.InnerText = "true";
                }
                if (nameGenderVisionDream.Value.Item3)
                {
                    XmlNode dreamNode = xmlDocument.CreateElement("person", "hadDream", "http://www.rdfbible.com/dev2/ns/person.owl");
                    person.AppendChild(dreamNode);
                    dreamNode.InnerText = "true";
                }
            }

            xmlDocument.Save(outputFile);
        }

        public static string GetString(SqlDataReader reader, string name)
        {
            var sqlString = reader.GetSqlString(reader.GetOrdinal(name));
            if (sqlString.IsNull) return null;
            return sqlString.Value;
        }
    }
}
