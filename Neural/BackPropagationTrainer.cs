﻿using System;
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

        public void Calculate()
        {
            this.Network.Calculate();
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

        private void CalculateInnerLayerWeightsError(Layer layer, Layer nextLayer)
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

        public bool Train(double[] target, double fidelity)
        {
            this.Network.Calculate();
            CalculateTotalError(target);

            if (this.ETotal < fidelity)
            {
                return true;
            }



            return false;
        }

        public BackPropagationTrainer(Network net, double speed = 0.5)
        {
            this.Network = net;
            this.Speed = speed;
        }
    }
}
