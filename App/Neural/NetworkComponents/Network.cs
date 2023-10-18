using System.Text.Json;
using System.Text.Json.Serialization;

namespace SnakeGame.App.Neural.NetworkComponents
{
    public class Network
    {
        public bool isTrainingMode = false;

        public string Name { get; set; }
        public List<Layer> Layers { get; set; }
        public int ValueOfInput { get; set; }
        public int ValueOfLearningCycles { get; set; }
        public double SumForAvgError { get; set; }
        public double TotalError { get; set; }
        public double AvgError { get; set; }
        public List<double> HistoryOfAvgError { get; set; }
        [JsonIgnore]
        public List<Value> Inputs { get; set; }
        [JsonIgnore]
        public List<Value> Outputs { get; set; }
        [JsonIgnore]
        public int[] NeuronsInLayer { get; set; }


        public List<Value> Calculate(double[] inputsVector)
        {
            UpdateInputs(inputsVector);

            Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron => neuron.CalculateValue());
            });

            return this.Outputs;
        }

        private void InitializeInputs(int valueOfInputs)
        {
            Inputs = new List<Value>();
            for (var i = 0; i < valueOfInputs; i += 1)
            {
                Inputs.Add(new Value(i, 0.0));
            }
        }

        private void UpdateInputs(double[] inputValues)
        {
            var i = 0;
            Inputs.ForEach((input) =>
            {
                input.Double = inputValues[i];
                i += 1;
            });
        }

        private void InitializeLayer(Layer layer, List<Value> inputs)
        {
            layer.InitializeNeuronsSynapses(inputs);
        }
        private void InitializeLayers()
        {
            InitializeLayer(Layers[0], Inputs);

            for (var i = 1; i < Layers.Count; i += 1)
            {
                InitializeLayer(Layers[i], Layers[i - 1].Outputs);
            }
        }
        private void InitializeLayers(int[] neuronsInLayers)
        {
            Layers.Add(new Layer(Inputs, neuronsInLayers[0]));

            for (var i = 1; i < neuronsInLayers.Length; i += 1)
            {
                Layers.Add(new Layer(Layers.Last().Outputs, neuronsInLayers[i])); //  проверить в каком порядке добавляются в список и поправить на фирст
            }
        }
        private void InitializeOutputs()
        {
            Outputs = new List<Value>();
            Layers.Last().Neurons.ForEach(neuron => Outputs.Add(neuron.OutputValue));
        }

        public static Network ReadNetworkFromFileOrCreate(string name, int valueOfInputs, int[] neuronsInLayer)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{name}.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, JsonSerializer.Serialize(new Network(name, valueOfInputs, neuronsInLayer)));
            }

            var data = File.ReadAllText(path);
            var network = JsonSerializer.Deserialize<Network>(data);
            return network;
        }

        public Network(string name, int valueOfInputs, int[] neuronsInLayers)
        {
            Name = name;
            ValueOfInput = valueOfInputs;
            NeuronsInLayer = neuronsInLayers;
            Layers = new List<Layer>();
            SumForAvgError = 0;
            AvgError = 1;
            ValueOfLearningCycles = 1;
            HistoryOfAvgError = new List<double>();

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
            Name = name;
            ValueOfLearningCycles = valueOfLearningCycles;
            Layers = layers;
            ValueOfInput = valueOfInput;
            Outputs = new List<Value>();
            HistoryOfAvgError = historyOfAvgError;
            SumForAvgError = sumForAvgError;
            AvgError = avgError;

            InitializeInputs(valueOfInput);
            InitializeLayers();
            InitializeOutputs();

        }
    }
}