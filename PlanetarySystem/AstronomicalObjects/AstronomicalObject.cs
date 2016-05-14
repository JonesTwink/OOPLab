using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlanetarySystem.AstronomicalObjects
{
    [Serializable]
    public abstract class AstronomicalObject
    {
        public string Name { get; set; }
        public double Mass { get; set; }
        public double Radius { get;  set; } 
        public double Temperature { get;  set; }
        public double RotationPeriod { get;  set; }
        public double DistanceToTheStar { get;  set; }
    }
}
