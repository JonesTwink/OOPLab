using System.IO;
using System.Collections.Generic;
using PlanetarySystem.AstronomicalObjects;

namespace PlanetarySystem
{
    public interface ISerializer
    {
        void Serialize(List<AstronomicalObject> ObjectsList, Stream target);
        List<AstronomicalObject> Deserialize(Stream source);
    }
}
