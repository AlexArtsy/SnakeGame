using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    public class BackPropagationTrainer : ITrainer
    {
        public Network Network { get; set; }
        public double[] Inputs { get; set; }
        public double[] Reference { get; set; }
        public double ETotal { get; set; }
        public double Speed { get; set; }

        private void CalculateTotalError(double[] target)
        {
            ETotal = 0;

            for (var i = 0; i < Network.Outputs.Count; i += 1)
            {
                ETotal += 0.5 * Math.Pow(target[i] - Network.Outputs[i].Double, 2);
            }
        }

        public void Calculate()
        {
            Network.Calculate();
        }

        private double CalculateNeuronOutputDelta(double output, double target)
        {
            return -1 * (target - output) * output * (1 - output);
        }

        private void CalculateLastLayerWeightsDelta(Layer layer, double[] target)
        {
            var i = 0;
            layer.Neurons.ForEach(n =>
            {
                n.DeltaOut = CalculateNeuronOutputDelta(n.OutputValue.Double, target[i]);

                n.Inputs.ForEach(input =>
                {
                    input.DeltaW = input.InputValue.Double * n.DeltaOut;
                });

                i += 1;
            });
        }

        private void CalculateInnerLayerWeightsDelta(Layer layer, Layer nextLayer)
        {
            layer.Neurons.ForEach(neuron =>
            {
                double sum = 0;
                nextLayer.Neurons.ForEach(neuronFromNextLayer => sum += neuronFromNextLayer.DeltaOut * neuronFromNextLayer.Inputs[neuron.Id].Weight);  //  проверить согласованы ли Id нейронов и Id синапсов!

                neuron.Inputs.ForEach(synapse =>
                {
                    synapse.DeltaW = sum
                                     * neuron.OutputValue.Double
                                     * (1 - neuron.OutputValue.Double)
                                     * synapse.InputValue.Double;
                });
            });
        }

        public double Train(double[] target)
        {
            Calculate();
            CalculateTotalError(target);
            CalculateLastLayerWeightsDelta(Network.Layers.Last(), target);

            for (var i = Network.Layers.Count - 2; i >= 0; i -= 1)
            {
                CalculateInnerLayerWeightsDelta(Network.Layers[i], Network.Layers[i + 1]);
            }

            Network.Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron =>
                {
                    neuron.Inputs.ForEach(input => input.UpdateWeight(this.Speed));
                });
            });

            return ETotal;
        }

        public BackPropagationTrainer(Network net, double speed = 0.5)
        {
            Network = net;
            Speed = speed;
        }
    }
}
