using System.IO;
using System.Collections.Generic;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PlanetarySystem
{
    class Converter
    {
        public static string ToJson(string source)
        {   
            string xmlData = LoadSource(source);
            XmlDocument doc = new XmlDocument();            
            doc.LoadXml(xmlData);

            string JsonString = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
            return JsonString;
            //return Regex.Replace(JsonString, "((?<=},)(?=.)|(?<={)(?=.)|(?<=\",)(?=.)|(?<=\")(?=})|(?<=.)(?={))", "\n\r");
                       
        }

        public static string FromJson(string source)
        {            
            XmlDocument doc = JsonConvert.DeserializeXmlNode(source);
            return doc.ToString();
        }

        private static string LoadSource(string source)
        {
            using (Stream stream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                return (new StreamReader(stream)).ReadToEnd();
            }            
        }
    }
}
