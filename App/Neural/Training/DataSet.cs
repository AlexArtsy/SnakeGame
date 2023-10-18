using SnakeGame.App.Field;

namespace SnakeGame.App.Neural.Training
{
    public class DataSet
    {
        public double[] Target { get; set; }
        public List<IFieldCellValue> InputData { get; set; }

        public DataSet(List<IFieldCellValue> input, double[] target)
        {
            InputData = input;
            Target = target;
        }
    }
}

