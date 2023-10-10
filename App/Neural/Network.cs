namespace SnakeGame.App.Neural
{
    public class Network
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        //public Layer InputLayer { get; set; }
        public List<Value> Outputs { get; set; } = new List<Value>();

        private void CreateNetwork(List<Value> inputs, int[] neuronsInLayers)
        {
            // this.InputLayer = new Layer(inputs, neuronsInLayers[0]);

            Layers.Add(new Layer(inputs, neuronsInLayers[0]));

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

        public Network(List<Value> inputs, int[] neuronsInLayers)
        {
            CreateNetwork(inputs, neuronsInLayers);
            InitializeOutputs();
        }
    }
}