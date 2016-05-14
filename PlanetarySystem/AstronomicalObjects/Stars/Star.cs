using System;
using System.Linq;

namespace PlanetarySystem.AstronomicalObjects.Stars
{
    [Serializable]
    public class Star : AstronomicalObject
    {
        public string StarType { get;  set; }
        public string StarColor { get;  set; }

        public void SetType(double StarRadiusArg)
        {
            if (StarRadiusArg <= 0.1)
            {
                StarType = StarProperties.StarTypes.Last().Name;
                Radius = StarRadiusArg;
            }
            else
                foreach (StarType Type in StarProperties.StarTypes)
                    if (StarRadiusArg >= Type.Radius)
                    {
                        StarType = Type.Name;
                        Radius = StarRadiusArg;
                        break;
                    }
               
        }

        public void SetSpectralClass(double StarTemperature)
        {
            if (StarTemperature >= 60000)
            {
                StarColor = StarProperties.SpectralClasses.Last().Color;
                Temperature = StarTemperature;
            } else
                foreach (SpectralClass StarClass in StarProperties.SpectralClasses)
                    if (StarTemperature < StarClass.Temperature)
                    {
                        StarColor = StarClass.Color;
                        Temperature = StarTemperature;
                        break;
                    }
        }

        public Star(string NameArg)
        {
            Name = NameArg;
            SetType(1);
            SetSpectralClass(StarProperties.SpectralClasses[1].Temperature);
            Mass = Radius*0.7;
        }
        public Star()
        {
        }
    }
}
