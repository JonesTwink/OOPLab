using System;

namespace PlanetarySystem.AstronomicalObjects.NonStarObjects.SmallBodies
{
    [Serializable]
    public abstract class SmallBody : NonStarObject
    {
        public string Satellites { get; set; }
    }
}
