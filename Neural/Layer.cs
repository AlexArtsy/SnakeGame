using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Neural;

namespace NeuroCompote
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; } = new List<Neuron>();
        //public List<double> Outputs { get; set; } = new List<double>();

        private void InitializeNeurons(List<Value> inputs, int valueOfNeurons)
        {
            for (var i = 0; i < valueOfNeurons; i += 1)
            {
                this.Neurons.Add(new Neuron(i, inputs, new Sigmoid(0.5)));
            }
        }
        private void InitializeNeurons(Layer inputLayer, int valueOfNeurons)
        {
            for (var i = 0; i < valueOfNeurons; i += 1)
            {
                this.Neurons.Add(new Neuron(i, inputLayer, new Sigmoid(0.5)));
            }
        }

        public Layer(Layer inputLayer, int valueOfNeurons)
        {
            InitializeNeurons(inputLayer, valueOfNeurons);
            //this.Neurons.ForEach(neuron => this.Outputs.Add(neuron.OutputValue));
        }

        public Layer(List<Value> inputs, int valueOfNeurons)   //  Входной слой (без нейронов)
        {
            InitializeNeurons(inputs, valueOfNeurons);
        }
    }
}
