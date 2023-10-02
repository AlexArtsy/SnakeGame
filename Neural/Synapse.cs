using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroCompote
{
    public class Synapse
    {
        private double value;
        //public double InitializedWeight { get; set; }
        //public double InitializedValue { get; set; }
        public double Weight { get; set; }
        public int Id { get; set; }

        public double Value
        {
            get => value;
            set
            {
                if (value >= 0 & value <= 1.0)
                {
                    this.value = value;
                }
                else
                {
                    throw new Exception($"Value of synapse must be between 0 and 1, value = {value}");
                }
            }
        }

        private void InitializeWeight()
        {
            this.Weight = RndGen.GetWeight();
        }

        public void AdjustWeightWithRandom()
        {
            this.Weight += RndGen.AdjustWeight();
        }

        public Synapse(int id, double value)
        {
            this.Id = id;
            this.Value = value;
            InitializeWeight();
        }
    }
}
