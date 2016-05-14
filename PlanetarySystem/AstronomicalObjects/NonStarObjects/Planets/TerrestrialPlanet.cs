using System;

namespace PlanetarySystem.AstronomicalObjects.NonStarObjects.Planets
{
    [Serializable]
    public class TerrestrialPlanet : Planet
    {

        public string Сomposition { get; set; }
        public bool FluidWater { get; set; }

        public TerrestrialPlanet(string NameArg, AstronomicalObject Center, double Distance)
        {
            Name = NameArg;
            CentralBody = Center;
            DistanceToTheStar = Distance;
            Radius = 0.5;
            Сomposition = "Железо-силикатная";
            PrevailngElement = "Азот";
            FluidWater = false;
            Mass = 0.1;
            Temperature = 10;
            OrbitalPeriod = DistanceToTheStar * 3;
            RotationPeriod = Radius * Mass * DistanceToTheStar;
            AtmosphereDepth = Radius / 8;
        }
        public TerrestrialPlanet()
        {
        }
    }
}
