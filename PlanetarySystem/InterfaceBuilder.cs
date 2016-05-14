using System;
using System.Collections.Generic;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Controls;
using PlanetarySystem.AstronomicalObjects.NonStarObjects.SmallBodies;
using PlanetarySystem.AstronomicalObjects.NonStarObjects.Planets;
using PlanetarySystem.AstronomicalObjects.Stars;
using PlanetarySystem.AstronomicalObjects;

namespace PlanetarySystem
{
    static class InterfaceBuilder
    {


        public static void  getGraphicalObjectProperties(AstronomicalObject CelestialBody, out int Distance, out Brush Color, out int Radius)
        {
            Distance = 0;
            Color = Brushes.White;
            Radius = 1;

            switch (TrimClassName(CelestialBody.GetType().ToString()))
            {
                case "Star":
                    Color = Brushes.Yellow;
                    Radius = 10;
                    Distance = 0;
                    break;
                case "TerrestrialPlanet":
                    Distance = (int)((TerrestrialPlanet)CelestialBody).DistanceToTheStar;
                    Color = Brushes.DarkSeaGreen;
                    Radius = 5;
                    break;

                case "GasGiant":
                    Distance = (int) ((GasGiant)CelestialBody).DistanceToTheStar;
                    Color = Brushes.LightSalmon;
                    Radius = 7;
                    break;

                case "Comet":
                    Distance = (int)((Comet)CelestialBody).DistanceToTheStar;
                    Color = Brushes.Gray;
                    Radius = 1;
                    break;

                case "Asteroid":
                    Distance = (int)((Asteroid)CelestialBody).DistanceToTheStar;
                    Color = Brushes.DarkGray;
                    Radius = 2;
                    break;
            }
        }

        public static string TrimClassName(string Name)
        {
            int i = Name.Length - 1;
            while (Name[i] != '.' && i>0)
                i--;
            Name = Name.Remove(0, i+1);
            return Name;
        }
       
    }
}
