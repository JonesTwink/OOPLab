using System;
using System.Collections.Generic;
using System.IO;
using PlanetarySystem.AstronomicalObjects.NonStarObjects.SmallBodies;
using PlanetarySystem.AstronomicalObjects.NonStarObjects.Planets;
using PlanetarySystem.AstronomicalObjects.Stars;
using PlanetarySystem.AstronomicalObjects;

namespace PlanetarySystem
{
    class XmlSerializer  : ISerializer
    {
        System.Xml.Serialization.XmlSerializer serializer;

        public void Serialize(List<AstronomicalObject> ObjectsList, Stream target)
        {
                serializer.Serialize(target, ObjectsList);         
        }

        public List<AstronomicalObject> Deserialize(Stream source)
        {            
            return (List<AstronomicalObject>)serializer.Deserialize(source);
        }

        public Type[] GetXMLExtraTypes()
        {
            List<Type> TypesArray = new List<Type>();
            TypesArray.Add(typeof(Star));
            TypesArray.Add(typeof(GasGiant));
            TypesArray.Add(typeof(Comet));
            TypesArray.Add(typeof(Asteroid));
            TypesArray.Add(typeof(TerrestrialPlanet));
            return TypesArray.ToArray();
        }

        public XmlSerializer()
        {
            List<AstronomicalObject> targetType = new List<AstronomicalObject>();
            serializer = new System.Xml.Serialization.XmlSerializer(targetType.GetType(), GetXMLExtraTypes());
        }

    }
}
