using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetarySystem.AstronomicalObjects;
using System.IO;

namespace PlanetarySystem.CommandPattern
{
    public sealed class SerializeCommand : Command
    {
        public ISerializer serializer { get; set; }
        public List<AstronomicalObject> item;
        public Stream stream { get; set; }

        public override void Execute()
        {
            if (stream != null)
                serializer.Serialize(item, stream);
        }

        public SerializeCommand(ISerializer serializer, List<AstronomicalObject> item)
        {
            this.serializer = serializer;
            this.item = item;
            this.stream = stream;
        }
    }
}
