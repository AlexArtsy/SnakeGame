using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    public interface IActivation
    {
        public double Calculate(Value x);
    }
}
