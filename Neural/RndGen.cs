using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroCompote
{
    internal class RndGen
    {
        private static Random rnd = new Random();

        public static double GetWeight()
        {
            return 1 - rnd.NextDouble() * 2;
        }

        public static double AdjustWeight()
        {
            return GetWeight() * 0.01;
        }
    }
}
