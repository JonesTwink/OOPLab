
namespace PlanetarySystem.AstronomicalObjects.Stars
{
    class StarType
    {
        public string Name { get;  set; }
        public double Radius { get;  set; }

        public StarType(string NameArg, double RadiusArg)
        {
            Name = NameArg;
            Radius = RadiusArg;
        }
    }
}
