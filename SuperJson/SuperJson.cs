using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text.RegularExpressions;

namespace SuperJson
{
    public class SuperJson : PluginInterface.IPlugin
    {

        private const string ext = "super";
        public string extension
        {
            get
            {
                return ext;
            }
        }

        public string ToJson(string source)
        {
            string xmlData = LoadSource(source);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);

            string JsonString = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);

            return Transform(JsonString);

        }

        public string FromJson(string source)
        {
            string JsonString = Detransform(LoadSource(source));

            XmlDocument doc = JsonConvert.DeserializeXmlNode(JsonString);
            return doc.OuterXml;
        }

        private string LoadSource(string source)
        {
            using (Stream stream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                return (new StreamReader(stream)).ReadToEnd();
            }
        }

        private string Transform(string data)
        {
            data = Regex.Replace(data, "(?<= )(?=\".*\":)", "<");
            data = Regex.Replace(data, "(?<=\".*\")(?=[,\r])", ">");
            data = Regex.Replace(data, "\"", "\'");       
            return data;
        }
        private string Detransform(string data)
        {
            data = Regex.Replace(data, "\'", "\"");
            data = Regex.Replace(data, "(?<= )(<)(?=\".*\":)", "");
            data = Regex.Replace(data, "(?<=\".*\")(>)(?=[,\r])", "");
            return data;
        }
    }
}
