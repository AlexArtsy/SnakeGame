using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal interface IFieldCell
    {
        //FieldCoordinates Position { get; set; }
        string Figure { get; set; }
        ConsoleColor Color { get; set; }
        ConsoleColor BgColor { get; set; }

        void Consume(Snake.Snake snake, FieldCell cell);

        //#region Делегаты
        //delegate void IFieldCellHandler(IFieldCell value);
        //#endregion

        //#region События
        //event IFieldCellHandler Changed;
        //#endregion
    }
}
