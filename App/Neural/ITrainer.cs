using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    internal interface ITrainer
    {
        Network Network { get; set; }
        double[] Inputs { get; set; }
        double[] Reference { get; set; }

        public bool Train(double[] target);
    }
}
