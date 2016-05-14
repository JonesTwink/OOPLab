using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetarySystem.AdapterPattern
{
    public class PluginManager
    {
        private List<PluginInterface.IPlugin> pluginList;


        public void TryToUsePlugins(string filename)
        {
            PluginInterface.IPlugin plugin = ChoosePlugin(InterfaceBuilder.TrimClassName(filename));
            if (plugin == null)
            {
                MessageBox.Show(string.Format("Плагин для данного типа файла не найден."), "Ошибка при загрузке плагина");
                return;
            }
            string xmlData = plugin.FromJson(filename);

            using (Stream stream = new FileStream("Json.xml", FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(xmlData);
                }
            }
        }

        public void SaveWithPlugin(string name)
        {

            PluginInterface.IPlugin plugin = ChoosePluginFromList(name);
            string jsonData = plugin.ToJson("DataForJsonPlugin.xml");

            using (Stream stream = new FileStream("Json." + plugin.extension, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(jsonData);
                }
            }


        }

        public PluginInterface.IPlugin ChoosePlugin(string ext)
        {
            PluginInterface.IPlugin plugin = null;

            foreach (PluginInterface.IPlugin p in pluginList)
            {
                if (p.extension == ext)
                {
                    plugin = p;
                    break;
                }
            }
            return plugin;
        }

        private PluginInterface.IPlugin ChoosePluginFromList(string name)
        {
            PluginInterface.IPlugin plugin = null;

            foreach (PluginInterface.IPlugin p in pluginList)
            {
                if (p.GetType().Name == name)
                {
                    plugin = p;
                    break;
                }
            }
            return plugin;
        }

        public PluginManager(List<PluginInterface.IPlugin> pluginList)
        {
            this.pluginList = pluginList;
        }
    }
}
