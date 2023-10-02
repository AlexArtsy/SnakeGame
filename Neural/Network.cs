namespace NeuroCompote
{
    public class Network
    {
        public List<Layer> Layers { get; set; } = new List<Layer>();
        //public Layer InputLayer { get; set; }
        public List<double> Outputs { get; set; } = new List<double>();

        private void CreateNetwork(List<double> inputs, int[] neuronsInLayers)
        {
            // this.InputLayer = new Layer(inputs, neuronsInLayers[0]);

            this.Layers.Add(new Layer(inputs, neuronsInLayers[0]));

            for (var i = 1; i < neuronsInLayers.Length; i += 1)
            {
                this.Layers.Add(new Layer(this.Layers.Last(), neuronsInLayers[i])); //  проверить в каком порядке добавляются в список и поправить на фирст
            }
        }

        public void InitializeOutputs()
        {
            this.Layers.Last().Neurons.ForEach(neuron => Outputs.Add(neuron.OutputValue));
        }

        public void Calculate()
        {
            this.Layers.ForEach(layer =>
            {
                layer.Neurons.ForEach(neuron => neuron.CalculateValue());
            });
        }

        public Network(List<double> inputs, int[] neuronsInLayers)
        {
            CreateNetwork(inputs, neuronsInLayers);
            InitializeOutputs();
        }
    }
}