using System.Collections;
using System.Collections.Generic;
using PlanetarySystem.AstronomicalObjects;
using PlanetarySystem.AstronomicalObjects.Stars;

namespace PlanetarySystem
{
    class PlanetarySystemObjects
    {
        public List<AstronomicalObject> ObjectsList { get; set; }

        public int Add(Star star)
        {
            if (ObjectsList.Capacity == 0)
            {
                ObjectsList.Add(star);
                return 1;
            }                
            else
                return 0;
        }

        public int Remove(Star star)
        {
            if (ObjectsList.Capacity > 0)
            {
                ObjectsList.Clear();
                return 1;
            }
            else
                return 0;
        }

        public int Add(AstronomicalObject NonStar)
        {           
                ObjectsList.Add(NonStar);
                return 1; 
        }

        public int Remove(AstronomicalObject NonStar)
        {
            if (ObjectsList.Capacity > 0)
            {
                ObjectsList.Remove(NonStar);
                return 1;
            }
            else
                return 0;
        }

        public PlanetarySystemObjects()
        {
            ObjectsList = new List<AstronomicalObject>();
        }
    }
}
