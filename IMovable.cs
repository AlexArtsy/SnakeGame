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
        FieldCoordinates Position { get; set; }
        FieldCoordinates NextPosition { get; set; }

        void Move(GameField field);

    }
}
