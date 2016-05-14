using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetarySystem.AstronomicalObjects;
using System.IO;

namespace PlanetarySystem.CommandPattern
{
    class SerializationManager
    {
        public SerializeCommand serializeCommand { get; set; }
        public DeserializeCommand deserializeCommand { get; set; }
        
        public void LaunchSerialization(string filename)
        {
            if (serializeCommand != null)
                using (Stream stream = new FileStream(filename, FileMode.Create, FileAccess.Write))
                {
                    serializeCommand.stream = stream;
                    serializeCommand.Execute();
                }
        }

        public List<AstronomicalObject> LaunchDeserialization(string filename)
        {
            if (deserializeCommand != null)
            {
                using (Stream stream = new FileStream(filename, FileMode.Open, FileAccess.Read))
                {
                    deserializeCommand.stream = stream;
                    if (stream != null)
                        deserializeCommand.Execute();
                }
                return deserializeCommand.ShowResult();
            }
            else return null;      
        }

        public SerializationManager()
        {
        }
        public SerializationManager(SerializeCommand serializeCommand)
        {
            this.serializeCommand = serializeCommand;
        }

        public SerializationManager(DeserializeCommand deserializeCommand)
        {
            this.deserializeCommand = deserializeCommand;
        }

        public SerializationManager(SerializeCommand serializeCommand, DeserializeCommand deserializeCommand):this(serializeCommand)
        {
            this.deserializeCommand = deserializeCommand;
        }

    }
}
