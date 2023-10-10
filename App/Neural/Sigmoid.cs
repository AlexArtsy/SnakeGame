using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    internal class Sigmoid : IActivation
    {
        private readonly double sigma;
        public double Calculate(Value x)
        {
            return 1.0 / (1.0 + Math.Exp(-1 * sigma * x.Double));
        }

        public Sigmoid(double sigma)
        {
            this.sigma = sigma;
        }
    }
}
