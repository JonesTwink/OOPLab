
namespace PlanetarySystem.AstronomicalObjects.Stars
{
    class SpectralClass
    {
        public string Color { get;  set; }
        public double Temperature { get;  set; }

        public SpectralClass(string ColorArg, double TemperatureArg)
        {
            Color = ColorArg;
            Temperature = TemperatureArg;
        }
    }
}
