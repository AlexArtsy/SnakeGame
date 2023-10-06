using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Neural;

namespace NeuroCompote
{
    public class Synapse
    {
        private Value inputValue;
        //public double InitializedWeight { get; set; }
        //public double InitializedValue { get; set; }
        public double Weight { get; set; }
        public int Id { get; set; }

        public Value InputValue { get; set; }
        //{
        //    get => inputValue;
        //    set
        //    {
        //        if (inputValue.Double >= 0 & inputValue.Double <= 1.0)
        //        {
        //            this.inputValue = value;
        //        }
        //        else
        //        {
        //            throw new Exception($"Value of synapse must be between 0 and 1, value = {value}");
        //        }
        //    }
        //}

        private void InitializeWeight()
        {
            this.Weight = RndGen.GetWeight();
        }

        public void AdjustWeightWithRandom()
        {
            this.Weight += RndGen.AdjustWeight();
        }

        public Synapse(int id, Value value)
        {
            this.Id = id;
            this.InputValue = value;
            InitializeWeight();
        }
    }
}
