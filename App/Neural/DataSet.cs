using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Field;

namespace SnakeGame.App.Neural
{
    public class DataSet
    {
        public double[] Target { get; set; }
        public List<IFieldCellValue> InputData { get; set; }

        public DataSet(List<IFieldCellValue> input, double[] target)
        {
            this.InputData = input;
            this.Target = target;
        }
    }
}
