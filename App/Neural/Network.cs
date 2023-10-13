using System.Text.Json.Serialization;

namespace SnakeGame.App.Neural
{
    public class Network
    {
        public string Name { get; set; }
        public List<Layer> Layers { get; set; }
        public int ValueOfInput { get; set; }
        public int ValueOfLearningCycles { get; set; }
        public double SumForAvgError { get; set; }
        public double AvgError { get; set; }
        public List<double> HistoryOfAvgError { get; set; }
        [JsonIgnore]
        public List<Value> Inputs { get; set; }
        [JsonIgnore]
        public List<Value> Outputs { get; set; }
        [JsonIgnore]
        public int[] NeuronsInLayer { get; set; }


        public void Calculate()
        {
            Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron => neuron.CalculateValue());
            });
        }

        private void InitializeInputs(int valueOfInputs)
        {
            this.Inputs = new List<Value>();
            for (var i = 0; i < valueOfInputs; i += 1)
            {
                this.Inputs.Add(new Value(i, 0.0));
            }
        }

        private void InitializeLayer(Layer layer, List<Value> inputs)
        {
            layer.InitializeNeuronsSynapses(inputs);
        }
        private void InitializeLayers()
        {
            InitializeLayer(this.Layers[0], this.Inputs);

            for (var i = 1; i < this.Layers.Count; i += 1)
            {
                InitializeLayer(this.Layers[i], this.Layers[i - 1].Outputs);
            }
        }
        private void InitializeLayers(int[] neuronsInLayers)
        {
            Layers.Add(new Layer(this.Inputs, neuronsInLayers[0]));

            for (var i = 1; i < neuronsInLayers.Length; i += 1)
            {
                Layers.Add(new Layer(Layers.Last().Outputs, neuronsInLayers[i])); //  проверить в каком порядке добавляются в список и поправить на фирст
            }
        }
        private void InitializeOutputs()
        {
            this.Outputs = new List<Value>();
            Layers.Last().Neurons.ForEach(neuron => Outputs.Add(neuron.OutputValue));
        }

        public Network(string name, int valueOfInputs, int[] neuronsInLayers)
        {
            this.Name = name;
            this.ValueOfInput = valueOfInputs;
            this.NeuronsInLayer = neuronsInLayers;
            this.Layers = new List<Layer>();
            this.SumForAvgError = 0;
            this.AvgError = 1;
            this.ValueOfLearningCycles = 0;

            InitializeInputs(valueOfInputs);
            InitializeLayers(neuronsInLayers);
            InitializeOutputs();
        }

        [JsonConstructor]
        public Network(
            string name, 
            List<Layer> layers, 
            int valueOfInput, 
            int valueOfLearningCycles, 
            List<double> historyOfAvgError, 
            double sumForAvgError, 
            double avgError)
        {
            this.Name = name;
            this.ValueOfLearningCycles = valueOfLearningCycles;
            this.Layers = layers;
            this.ValueOfInput = valueOfInput;
            this.Outputs = new List<Value>();
            this.HistoryOfAvgError = historyOfAvgError;
            this.SumForAvgError = sumForAvgError;
            this.AvgError = avgError;
            
            InitializeInputs(valueOfInput);
            InitializeLayers();
            InitializeOutputs();
            
        }
    }
}