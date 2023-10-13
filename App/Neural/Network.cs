namespace SnakeGame.App.Neural
{
    public class Network
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        public List<Value> Inputs { get; set; } = new List<Value>();
        public List<Value> Outputs { get; set; } = new List<Value>();
        public int ValueOfInput { get; set; }
        public int[] NeuronsInLayer { get; set; }
        public string Name { get; set; }

        private void CreateNetwork(int[] neuronsInLayers)
        {
            // this.InputLayer = new Layer(inputs, neuronsInLayers[0]);

            Layers.Add(new Layer(this.Inputs, neuronsInLayers[0]));

            for (var i = 1; i < neuronsInLayers.Length; i += 1)
            {
                Layers.Add(new Layer(Layers.Last(), neuronsInLayers[i])); //  проверить в каком порядке добавляются в список и поправить на фирст
            }
        }

        public void InitializeOutputs()
        {
            Layers.Last().Neurons.ForEach(neuron => Outputs.Add(neuron.OutputValue));
        }

        public void Calculate()
        {
            Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron => neuron.CalculateValue());
            });
        }

        private void SetUpInputs(int valueOfInputs)
        {
            for (var i = 0; i < valueOfInputs; i += 1)
            {
                this.Inputs.Add(new Value(i, 0.0));
            }
        }

        public Network(string name, int valueOfInputs, int[] neuronsInLayers)
        {
            this.Name = name;
            this.ValueOfInput = valueOfInputs;
            this.NeuronsInLayer = neuronsInLayers;
            SetUpInputs(valueOfInputs);
            CreateNetwork(neuronsInLayers);
            InitializeOutputs();
        }
    }
}