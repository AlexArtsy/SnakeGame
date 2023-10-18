using System.Text.Json.Serialization;

namespace SnakeGame.App.Neural.NetworkComponents
{
    public class Neuron
    {
        public int Id { get; set; }
        public List<Synapse> Synapses { get; set; }
        [JsonIgnore]
        public List<Value> Inputs { get; set; }
        [JsonIgnore]
        public Value OutputValue { get; set; }
        [JsonIgnore]
        public IActivation ActivationFunc { get; set; }
        [JsonIgnore]
        public double DeltaOut { get; set; }


        private void InitializeSynapses()    //  inputs - выхода предидущего слоя
        {
            var i = 0;
            Inputs.ForEach(input =>
            {
                Synapses.Add(new Synapse(i, input));
                i += 1;
            });
        }
        public void InitializeSynapses(List<Value> inputs)
        {
            Inputs = inputs;

            for (var i = 0; i < inputs.Count; i += 1)
            {
                Synapses[i].InitializeInput(inputs[i]);
            }
        }

        public void AdjustSynapsesWithRandom(double maxValue = 0.05)
        {
            Synapses.ForEach(s => s.AdjustWeightWithRandom());
        }

        public void CalculateValue()
        {
            OutputValue.Double = ActivationFunc.Calculate(GetWeightedSum());
        }
        //public void CalculateValue(Value input)    //  для псевдо-нейронов-входов
        //{
        //    this.OutputValue = input;
        //}

        private Value GetWeightedSum()
        {
            double sum = 0;
            Synapses.ForEach(s =>
            {
                sum += s.Weight * s.InputValue.Double;
            });
            return new Value(0, sum);
        }

        #region Constructors
        public Neuron(int id, List<Value> inputs, IActivation activationFunc)    //  Для входного слоя
        {
            Id = id;
            Inputs = inputs;
            Synapses = new List<Synapse>();
            OutputValue = new Value(0, 0);
            ActivationFunc = activationFunc;
            InitializeSynapses();
            CalculateValue();
        }

        [JsonConstructor]
        public Neuron(int id, List<Synapse> synapses)
        {
            Id = id;
            Synapses = synapses;
            OutputValue = new Value(0, 0);
            ActivationFunc = new Sigmoid(0.5);
        }
        #endregion
    }
}
