using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal interface IMovable
    {
        int Speed { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        string CurrentDirection { get; set; }
        string NewDirection { get; set; }


        void Move();

    }
}
