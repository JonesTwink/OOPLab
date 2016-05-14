using System.Collections.Generic;

namespace PlanetarySystem.AstronomicalObjects.Stars
{
    static class StarProperties
    {
        public static List<StarType> StarTypes = new List<StarType>() { new StarType("Сверхгигант", 100), new StarType("Гигант", 10), new StarType("Звезда главной последовательности", 0.8), new StarType("Карлик", 0.1) };
        public static List<SpectralClass> SpectralClasses = new List<SpectralClass>() { new SpectralClass("Красный", 3500), new SpectralClass("Желтый", 6000), new SpectralClass("Белый", 10000), new SpectralClass("Голубой", 60000) };
    }
}
