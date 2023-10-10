using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    public class Synapse
    {
        private Value inputValue;
        //public double InitializedWeight { get; set; }
        //public double InitializedValue { get; set; }
        public double Weight { get; set; }
        public double DeltaW { get; set; }
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
        public void UpdateWeight(double learningSpeed)
        {
            Weight = Weight - learningSpeed * DeltaW;
        }
        private void InitializeWeight()
        {
            Weight = RndGen.GetWeight();
        }

        public void AdjustWeightWithRandom()
        {
            Weight += RndGen.AdjustWeight();
        }

        public Synapse(int id, Value value)
        {
            Id = id;
            InputValue = value;
            InitializeWeight();
        }
    }
}
