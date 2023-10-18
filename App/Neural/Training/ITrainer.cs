using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Neural.NetworkComponents;

namespace SnakeGame.App.Neural.Training
{
    internal interface ITrainer
    {
        Network Network { get; set; }
        double[] Inputs { get; set; }
        double[] Reference { get; set; }

        public double Train(double[] target);
    }
}
