using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuroCompote;

namespace SnakeGame.Neural
{
    internal class BackPropagationTrainer : ITrainer
    {
        public Network Network { get; set; }
        public double[] Inputs { get; set; }
        public double[] Reference { get; set; }
        public double ETotal { get; set; }
        public double Speed { get; set; }

        private void CalculateTotalError(double[] target)
        {

            for (var i = 0; i < this.Network.Outputs.Count; i += 1)
            {
                this.ETotal += 0.5 * Math.Pow(target[i] - this.Network.Outputs[i].Double, 2);
            }
        }
        public void Train()
        {
            
        }

        public BackPropagationTrainer(Network net, double speed = 0.5)
        {
            this.Network = net;
            this.Speed = speed;
        }
    }
}
