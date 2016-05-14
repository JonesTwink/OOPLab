using System;
using System.Collections.Generic;

namespace PlanetarySystem.AstronomicalObjects.NonStarObjects
{
    [Serializable]
    public abstract class NonStarObject : AstronomicalObject
    {
        public AstronomicalObject CentralBody { get;  set; }

        public bool Life { get; set; }
        new public double Radius { get; set; } 
        new public double Temperature { get; set; }
        public double OrbitalPeriod { get; set; }
        new public double DistanceToTheStar { get; set; }

    }
}
