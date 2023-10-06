using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Neural
{
    public class Value
    {
        public int Id { get; set; }
        public double Double { get; set; }  //  для передачи по ссылке примитивного значения
        public Value(int id, double value)
        {
            this.Id = id;
            this.Double = value;
        }
    }
}
