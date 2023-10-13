using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
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
            this.Inputs.ForEach(input =>
            {
                Synapses.Add(new Synapse(i, input));
                i += 1;
            });
        }
        public void InitializeSynapses(List<Value> inputs)
        {
            this.Inputs = inputs;

            for (var i = 0; i < inputs.Count; i += 1)
            {
                this.Synapses[i].InitializeInput(inputs[i]);
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
            this.Inputs = inputs;
            this.Synapses = new List<Synapse>();
            this.OutputValue = new Value(0, 0);
            this.ActivationFunc = activationFunc;
            InitializeSynapses();
            CalculateValue();
        }

        [JsonConstructor]
        public Neuron(int id, List<Synapse> synapses)
        {
            this.Id = id;
            this.Synapses = synapses;
            this.OutputValue = new Value(0, 0);
            this.ActivationFunc = new Sigmoid(0.5);
        }
        #endregion
    }
}
