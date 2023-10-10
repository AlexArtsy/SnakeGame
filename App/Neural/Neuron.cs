using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    public class Neuron
    {
        public readonly int layerId;

        public int Id { get; set; }
        public Value OutputValue { get; set; } = new Value(0, 0);   //  пересмотреть айди
        public IActivation ActivationFunc { get; set; }
        public List<Synapse> Inputs { get; set; } = new List<Synapse>();
        public double DeltaOut { get; set; }


        private void InitializeSynapses(List<Value> inputs)    //  inputs - выхода предидущего слоя
        {
            var i = 0;
            inputs.ForEach(input =>
            {
                Inputs.Add(new Synapse(i, input));
                i += 1;
            });
        }
        private void InitializeSynapses(Layer inputLayer)
        {
            var i = 0;
            inputLayer.Neurons.ForEach(neuron =>
            {
                Inputs.Add(new Synapse(i, neuron.OutputValue));
                i += 1;
            });
        }

        public void AdjustSynapsesWithRandom(double maxValue = 0.05)
        {
            Inputs.ForEach(s => s.AdjustWeightWithRandom());
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
            Inputs.ForEach(s =>
            {
                sum += s.Weight * s.InputValue.Double;
            });
            return new Value(0, sum);
        }

        #region Constructors
        public Neuron(int id, Layer inputLayer, IActivation activationFunc)
        {
            Id = id;
            //this.layerId = layerId;
            ActivationFunc = activationFunc;
            InitializeSynapses(inputLayer);
            CalculateValue();
        }

        public Neuron(int id, List<Value> inputs, IActivation activationFunc)    //  Для входного слоя
        {
            Id = id;
            ActivationFunc = activationFunc;
            InitializeSynapses(inputs);
            CalculateValue();

        }
        #endregion
    }
}
