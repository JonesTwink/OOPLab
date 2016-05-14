using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using PlanetarySystem.AstronomicalObjects;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace PlanetarySystem
{
    class BinarySerializer : ISerializer
    {
        BinaryFormatter Serializer = new BinaryFormatter();

        public void Serialize(List<AstronomicalObject>ObjectsList, Stream target)
        {            
            Serializer.Serialize(target, ObjectsList);            
        }

        public List<AstronomicalObject> Deserialize(Stream source)
        {
                return (List<AstronomicalObject>)Serializer.Deserialize(source);            
        } 
    }
}
