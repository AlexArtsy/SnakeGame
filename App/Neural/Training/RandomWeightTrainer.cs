using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Neural.NetworkComponents;

namespace SnakeGame.App.Neural.Training
{
    internal class RandomWeightTrainer : ITrainer
    {
        public Network Network { get; set; }
        public double[] Inputs { get; set; }
        public double[] Reference { get; set; }

        public double Train(double[] target)
        {
            Network.Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron =>
                {
                    neuron.Synapses.ForEach(input =>
                    {
                        input.AdjustWeightWithRandom();
                    });
                });
            });

            return 0.0;
        }

        public RandomWeightTrainer(Network network)
        {
            Network = network;
        }
    }
}
