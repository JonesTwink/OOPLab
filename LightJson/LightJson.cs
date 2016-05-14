using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text.RegularExpressions;

namespace LightJson
{
    public class LightJson : PluginInterface.IPlugin
    {
        private const string ext = "light";
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
            data = Regex.Replace(data, ": ", " = ");
            data = Regex.Replace(data, "(?<= )(\")(?=[@?A-Za-zА-Яа-я0-9-])|(?<=[A-Za-zА-Яа-я0-9-])(\")(?=[ ,\r\t])", "");
            data = Regex.Replace(data, ",", ";");
            return data;
        }
        private string Detransform(string data)
        {
            data = Regex.Replace(data, ";", ",");
            data = Regex.Replace(data, "(?<= )(?=[@?A-Za-zА-Яа-я0-9-])|(?<=[A-Za-zА-Яа-я0-9-])(?=[ ,\r\t])", "\"");
            data = Regex.Replace(data, " = ", ": ");
            return data;
        }
    }
}
