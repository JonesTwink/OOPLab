using System;

namespace PlanetarySystem.AstronomicalObjects.NonStarObjects.SmallBodies
{
    [Serializable]
    public class Asteroid : SmallBody
    {

        public string Composition { get; set; }
        public string Shape { get; set; }

        public Asteroid(string NameArg, AstronomicalObject Center, double Distance)
        {
            Name = NameArg;
            CentralBody = Center;
            DistanceToTheStar = Distance;
            Radius = 0.3;
            Mass = 0.01;
            Temperature = -400;
            OrbitalPeriod = DistanceToTheStar * 3;
            Composition = "Углеродный";
            Shape = "Неправильная";
        }
        public Asteroid()
        {
        }
    }
}
