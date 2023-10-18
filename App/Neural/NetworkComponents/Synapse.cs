using System.Text.Json.Serialization;

namespace SnakeGame.App.Neural.NetworkComponents
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
            InputValue = input;
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
            Id = id;
            Weight = weight;
        }
    }
}
