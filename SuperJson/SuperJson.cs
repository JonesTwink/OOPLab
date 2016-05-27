using System.IO;
using Newtonsoft.Json;
using System.Xml;
using System.Text.RegularExpressions;

namespace SuperJson
{
    public class SuperJson : Decorator.Decorator
    {

        public SuperJson()
        {
            regularPlugin = new RegularJson.RegularJson();
        }
        private const string ext = "super";
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
            using (Stream stream = new FileStream(Directory.GetCurrentDirectory()+"\\temp.json", FileMode.Create, FileAccess.Write))
            {
                (new StreamWriter(stream)).Write(regularJsonData);
            }
            
            return base.FromJson(Directory.GetCurrentDirectory() + "\\temp.json");
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
        private string LoadSource(string source)
        {
            using (Stream stream = new FileStream(source, FileMode.Open, FileAccess.Read))
            {
                return (new StreamReader(stream)).ReadToEnd();
            }
        }
    }
}
