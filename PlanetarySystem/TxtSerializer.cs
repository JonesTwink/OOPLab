using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using PlanetarySystem.AstronomicalObjects;

namespace PlanetarySystem
{
    class TxtSerializer : ISerializer
    {
        public void Serialize( List<AstronomicalObject> ObjectsList, Stream target)
        {
            using (StreamWriter writer = new StreamWriter(target))
            {
                
                foreach (AstronomicalObject obj in ObjectsList)
                {
                    writer.WriteLine(obj.GetType().FullName);
                    PropertyInfo[] properties = obj.GetType().GetProperties();

                    foreach (PropertyInfo property in properties)
                    {
                        writer.WriteLine(property.GetValue(obj));
                    }                    

                }
            }
        }

        public List<AstronomicalObject> Deserialize(Stream source)
        {
            string objectType;            
            List<AstronomicalObject> ObjectsList = new List<AstronomicalObject>();

            List<string> fileContents = ReadFileContents(source);
            while (fileContents.Count > 0)
            {
                objectType = fileContents[0];
                fileContents.RemoveAt(0);

                var newObject = Activator.CreateInstance(Type.GetType(objectType));

                AddNewObject(newObject, fileContents, ObjectsList);
            }
            return ObjectsList;
        }


        private List<string> ReadFileContents(Stream source)
        {
            List<string> fileContents = new List<string>();
            using (StreamReader reader = new StreamReader(source))
            {
                while (!reader.EndOfStream)
                    fileContents.Add(reader.ReadLine());
            }
            return fileContents;
        }

        private void AddNewObject(object newObject, List<string> fileContents, List<AstronomicalObject> ObjectsList)
        {
            foreach (PropertyInfo property in newObject.GetType().GetProperties())
            {
                if (property.PropertyType == typeof(double))
                    property.SetValue(newObject, Double.Parse(fileContents[0]));
                else
                    if (property.PropertyType == typeof(bool))
                    property.SetValue(newObject, Boolean.Parse(fileContents[0]));
                else
                    if (property.PropertyType == typeof(AstronomicalObject))
                    property.SetValue(newObject, ObjectsList[0]);
                else
                    property.SetValue(newObject, fileContents[0]);
                fileContents.RemoveAt(0);
            }
            ObjectsList.Add((AstronomicalObject)newObject);
        }
    }
}
