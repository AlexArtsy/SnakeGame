using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal interface IFieldCell
    {
        int X { get; set; }
        int Y { get; set; }
        string Value { get; set; }
    }
}
