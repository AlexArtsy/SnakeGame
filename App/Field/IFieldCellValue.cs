using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Field
{
    public interface IFieldCellValue
    {
        //FieldCoordinates Position { get; set; }
        string Figure { get; set; }
        ConsoleColor Color { get; set; }
        ConsoleColor BgColor { get; set; }

        void Consume(Snake snake, FieldCell cell);

        //#region Делегаты
        //delegate void IFieldCellHandler(IFieldCell value);
        //#endregion

        //#region События
        //event IFieldCellHandler Changed;
        //#endregion
    }
}
