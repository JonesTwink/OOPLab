using System;

namespace PlanetarySystem.AstronomicalObjects.NonStarObjects.Planets
{
    [Serializable]
    public abstract class Planet : NonStarObject
    {
        public string Satellites { get;  set; }
        public double AtmosphereDepth { get; set; }
        public string PrevailngElement { get; set; }

        public string GetSatellites()
        {
            string SatellitesList = "";
            if (Satellites == null)
                SatellitesList = "Отсутствуют";

            return SatellitesList;

        }
    }
}
