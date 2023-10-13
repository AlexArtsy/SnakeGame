using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    public class Synapse
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        [JsonIgnore]
        public double DeltaW { get; set; }
        [JsonIgnore]
        public Value InputValue { get; set; }

        public void UpdateWeight(double learningSpeed)
        {
            Weight -= learningSpeed * DeltaW;
        }
        private void InitializeWeight()
        {
            Weight = RndGen.GetWeight();
        }

        public void InitializeInput(Value input)
        {
            this.InputValue = input;
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

        [JsonConstructor]
        public Synapse(int id, double weight)
        {
            this.Id = id;
            this.Weight = weight;
        }
    }
}
