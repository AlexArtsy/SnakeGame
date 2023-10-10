using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    internal class RandomWeightTrainer : ITrainer
    {
        public Network Network { get; set; }
        public double[] Inputs { get; set; }
        public double[] Reference { get; set; }

        public bool Train(double[] target)
        {
            Network.Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron =>
                {
                    neuron.Inputs.ForEach(input =>
                    {
                        input.AdjustWeightWithRandom();
                    });
                });
            });

            return true;
        }

        public RandomWeightTrainer(Network network)
        {
            Network = network;
        }
    }
}
