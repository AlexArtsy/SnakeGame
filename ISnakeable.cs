using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal interface ISnakeable
    {
        string Figure { get; set; }
        //int Speed { get; set; }
        ConsoleColor Color { get; set; }
        ConsoleColor BgColor { get; set; }
    }
}
