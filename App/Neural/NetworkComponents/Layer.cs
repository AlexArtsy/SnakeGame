using System.Text.Json.Serialization;

namespace SnakeGame.App.Neural.NetworkComponents
{
    public class Layer
    {
        public List<Neuron> Neurons { get; set; }
        [JsonIgnore]
        public List<Value> Outputs { get; set; }

        private void InitializeNeurons(List<Value> inputs, int valueOfNeurons)
        {
            for (var i = 0; i < valueOfNeurons; i += 1)
            {
                var neuron = new Neuron(i, inputs, new Sigmoid(0.5));
                Outputs.Add(neuron.OutputValue);
                Neurons.Add(neuron);
            }
        }

        public void InitializeNeuronsSynapses(List<Value> inputs)
        {
            Neurons.ForEach(n =>
            {
                n.InitializeSynapses(inputs);
                //this.Outputs.Add(n.OutputValue);
            });
        }

        private void InitializeOutputs()
        {
            Outputs = new List<Value>();
            Neurons.ForEach(n =>
            {
                Outputs.Add(n.OutputValue);
            });
        }
        //private void InitializeNeurons(Layer inputLayer, int valueOfNeurons)
        //{
        //    for (var i = 0; i < valueOfNeurons; i += 1)
        //    {
        //        Neurons.Add(new Neuron(i, inputLayer, new Sigmoid(0.5)));
        //    }
        //}

        //public Layer(Layer inputLayer, int valueOfNeurons)
        //{
        //    InitializeNeurons(inputLayer, valueOfNeurons);
        //    //this.Neurons.ForEach(neuron => this.Outputs.Add(neuron.OutputValue));
        //}

        public Layer(List<Value> inputs, int valueOfNeurons)
        {
            Neurons = new List<Neuron>();
            Outputs = new List<Value>();

            InitializeNeurons(inputs, valueOfNeurons);
        }

        [JsonConstructor]
        public Layer(List<Neuron> neurons)
        {
            Neurons = neurons;
            InitializeOutputs();
        }
    }
}
