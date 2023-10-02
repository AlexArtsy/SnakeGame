using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroCompote
{
    internal class RandomWeightTrainer : ITrainer
    {
        public Network Network { get; set; }
        public double[] Inputs { get; set; }
        public double[] Reference { get; set; }

        public void Train()
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
        }

        public RandomWeightTrainer(Network network)
        {
            this.Network = network;
        }
    }
}
