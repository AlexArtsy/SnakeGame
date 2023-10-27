using SnakeGame.App.Field;

namespace SnakeGame.App.Neural.Training
{
    public class DataSet
    {
        public double[] Target { get; set; }
        public double[] InputData { get; set; }

        public DataSet(double[] inputData, double[] target)
        {
            InputData = inputData;
            Target = target;
        }
    }
}

