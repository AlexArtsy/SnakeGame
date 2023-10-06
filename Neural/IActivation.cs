using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Neural;

namespace NeuroCompote
{
    public interface IActivation
    {
        public double Calculate(Value x);
    }
}
