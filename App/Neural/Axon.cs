using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    internal class Axon //  Возможно не нужен, т.к. это и есть синапс, а такая сущность уже есть.
    {
        public double Value { get; set; }

        public Axon(double value)
        {
            Value = value;
        }
    }
}
