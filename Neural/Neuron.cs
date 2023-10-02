using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroCompote
{
    public class Neuron
    {
        public readonly int layerId;

        public int Id { get; set; }
        public double OutputValue { get; set; }
        public IActivation ActivationFunc { get; set; }
        public List<Synapse> Inputs { get; set; } = new List<Synapse>();

        
        private void InitializeSynapses(List<double> inputs)    //  inputs - выхода предидущего слоя
        {
            var i = 0;
            inputs.ForEach(input =>
            {
                this.Inputs.Add(new Synapse(i, input));
                i += 1;
            });
        }
        private void InitializeSynapses(Layer inputLayer)
        {
            var i = 0;
            inputLayer.Neurons.ForEach(neuron =>
            {
                this.Inputs.Add(new Synapse(i, neuron.OutputValue));
                i += 1;
            });
        }

        public void AdjustSynapsesWithRandom(double maxValue = 0.05)
        {
            this.Inputs.ForEach((s => s.AdjustWeightWithRandom()));
        }
        public void CalculateValue()
        {
            this.OutputValue = ActivationFunc.Calculate(GetWeightedSum());
        }
        public void CalculateValue(double input)    //  для псевдо-нейронов-входов
        {
            this.OutputValue = input;
        }

        private double GetWeightedSum()
        {
            double sum = 0;
            this.Inputs.ForEach(s =>
            {
                sum += s.Weight * s.Value;
            });
            return sum;
        }

        #region Constructors
        public Neuron(int id, Layer inputLayer, IActivation activationFunc)
        {
            this.Id = id;
            //this.layerId = layerId;
            this.ActivationFunc = activationFunc;
            InitializeSynapses(inputLayer);
            CalculateValue();
        }

        public Neuron(int id, List<double> inputs, IActivation activationFunc)    //  Для входного слоя
        {
            this.Id = id;
            this.ActivationFunc = activationFunc;
            InitializeSynapses(inputs);
            CalculateValue();

        }
        #endregion
    }
}
