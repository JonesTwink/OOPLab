using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetarySystem.AstronomicalObjects;
using System.IO;

namespace PlanetarySystem.CommandPattern
{
    public sealed class DeserializeCommand : Command
    {
        public ISerializer serializer { get; set; }
        List<AstronomicalObject> item;
        public Stream stream { get; set; }


        public override void Execute()
        {
            if (stream != null)
                item =  serializer.Deserialize(stream);
        }

        public List<AstronomicalObject> ShowResult()
        {
            return item;
        }

        public DeserializeCommand(ISerializer serializer, List<AstronomicalObject> item)
        {
            this.serializer = serializer;
            this.item = item;
            this.stream = stream;
        }

    }
}
