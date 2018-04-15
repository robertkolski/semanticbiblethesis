using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace SearchScripture
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                System.Console.Out.WriteLine("usage: SearchScripture filename regex");
                return;
            }

            string filename = args[0];
            string regExString = args[1];

            XmlDocument xml = new XmlDocument();
            xml.Load(filename);

            foreach (XmlNode node in xml.SelectNodes(@"//BIBLEBOOK/CHAPTER/VERS"))
            {
                if (Regex.IsMatch(node.InnerText, regExString))
                {
                    System.Console.Out.WriteLine("Book {0} Chapter {1} Verse {2} Text {3}",
                        node.ParentNode.ParentNode.Attributes["bname"].Value,
                        node.ParentNode.Attributes["cnumber"].Value,
                        node.Attributes["vnumber"].Value,
                        node.InnerText
                        );

                    if (regExString.Contains("(?<parent>") && regExString.Contains("?<child>"))
                    {
                        MatchCollection matches = Regex.Matches(node.InnerText, regExString);
                        foreach (Match m in matches)
                        {
                            System.Console.Out.WriteLine("parent = {0}", m.Groups["parent"].Value);
                            System.Console.Out.WriteLine("child = {0}", m.Groups["child"].Value);
                        }
                    }
                }
            }
        }
    }
}
