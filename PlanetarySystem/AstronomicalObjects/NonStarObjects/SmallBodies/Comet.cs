using System;
using PlanetarySystem.AstronomicalObjects.Stars;

namespace PlanetarySystem.AstronomicalObjects.NonStarObjects.SmallBodies
{
    [Serializable]
    public class Comet : SmallBody
    {

        public double TailLength { get; set; }
        public double OrbitSize { get; set; }
        public Comet(string NameArg, AstronomicalObject Center, double Distance)
        {
            Name = NameArg;
            CentralBody = (Star)Center;
            DistanceToTheStar = Distance;
            Radius = 0.2;
            TailLength = 0.01;
            Mass = 0.001;
            Temperature = -20;
            OrbitalPeriod = DistanceToTheStar * 3;
            OrbitSize = DistanceToTheStar;
        }
        public Comet()
        {
        }
    }
}
