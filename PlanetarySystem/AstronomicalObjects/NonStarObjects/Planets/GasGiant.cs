using System;

namespace PlanetarySystem.AstronomicalObjects.NonStarObjects.Planets
{
    [Serializable]
    public class GasGiant : Planet
    {   
        public GasGiant(string NameArg, AstronomicalObject Center, double Distance)
        {
            
            Name = NameArg;
            CentralBody = Center;
            PrevailngElement = "Гелий";
            DistanceToTheStar = Distance;
            Radius = 0.8;
            Mass = 0.5;
            Temperature = 100;
            OrbitalPeriod = DistanceToTheStar*3;
            RotationPeriod = Radius * Mass * DistanceToTheStar;
            AtmosphereDepth = Radius / 3;            

        }
        public GasGiant()
        {
        }
    }
}
