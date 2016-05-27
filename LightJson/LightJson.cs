using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text.RegularExpressions;

namespace LightJson
{
    public class LightJson : Decorator.Decorator
    {
        private const string ext = "light";
        public LightJson()
        {
            regularPlugin = new RegularJson.RegularJson();
        }
        public override string extension
        {
            get
            {
                return ext;
            }
        }

        public override string ToJson(string source)
        {            
            string regularJsonData = base.ToJson(source);
            string transformedJsonData = Transform(regularJsonData);
            return transformedJsonData;
        }

        public override string FromJson(string source)
        {            
            string regularJsonData = Detransform(LoadSource(source));
            using (StreamWriter file = new StreamWriter(Directory.GetCurrentDirectory() + "\\temp.json"))
            {
                file.Write(regularJsonData);
            }

            return base.FromJson(Directory.GetCurrentDirectory() + "\\temp.json");
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
