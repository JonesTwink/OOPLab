using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlanetarySystem.AstronomicalObjects;
using System.IO;

namespace PlanetarySystem.CommandPattern
{

    public abstract class Command
    {
        public abstract void Execute();      
    }
}
