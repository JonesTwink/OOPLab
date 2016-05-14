using System.IO;
using Newtonsoft.Json;
using System.Xml;

namespace LightJson
{
    public class RegularJson : PluginInterface.IPlugin
    {
        private const string ext = "regular";
        public string extension
        {
            get {
                    return ext;
                }
        }

        public string ToJson(string source)
        {
            string xmlData = LoadSource(source);
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlData);

            string JsonString = JsonConvert.SerializeXmlNode(doc, Newtonsoft.Json.Formatting.Indented);
            return JsonString;

        }

        public string FromJson(string source)
        {
            XmlDocument doc = JsonConvert.DeserializeXmlNode(LoadSource(source));
            return doc.OuterXml;
        }

        private string LoadSource(string source)
        {
            using (Stream stream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                return (new StreamReader(stream)).ReadToEnd();
            }
        }
    }
}
