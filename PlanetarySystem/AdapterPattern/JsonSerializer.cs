using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PlanetarySystem.AstronomicalObjects;

namespace PlanetarySystem.AdapterPattern
{
    public class JsonSerializer : ISerializer
    {
        private XmlSerializer xmlSerializer = new XmlSerializer();
        private PluginManager pluginManager;

        private string filename;

        public void Serialize(List<AstronomicalObject> ObjectsList, Stream target)
        {
            using (Stream stream = new FileStream("DataForJsonPlugin.xml", FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(ObjectsList, stream);
            }
                pluginManager.SaveWithPlugin(filename);
        }

        public List<AstronomicalObject> Deserialize(Stream source)
        {
            pluginManager.TryToUsePlugins(filename);

            using (Stream stream = new FileStream("Json.xml", FileMode.Open, FileAccess.Read))
            {
                return xmlSerializer.Deserialize(stream);
            }
        }

        public JsonSerializer(string filename, PluginManager pluginManager)
        {
            this.filename = filename;
            this.pluginManager = pluginManager;
        }
    }
}
