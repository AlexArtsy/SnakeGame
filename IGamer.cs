using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal interface IGamer
    {
        Snake.Snake Snake { get; set; }
        GameField Field { get; set; }
        void Play();
    }
}
