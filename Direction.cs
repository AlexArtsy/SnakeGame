using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Direction
    {
        public string Value { get; set; }

        public Direction(string value)
        {
            this.Value = value;
        }
    }
}
