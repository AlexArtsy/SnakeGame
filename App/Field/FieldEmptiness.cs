using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Field
{
    internal class FieldEmptiness : IFieldCellValue
    {
        #region Поля
        #endregion

        #region Свойства
        //public FieldCoordinates Position { get; set; }
        public string Figure { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы
        public void Consume(Snake snake, FieldCell cell)
        {
            return;
        }
        #endregion

        #region Делегаты
        // public delegate void IFieldCellHandler(IFieldCell value);
        #endregion

        #region События
        // public event IFieldCell.IFieldCellHandler Changed;
        #endregion

        #region Конструкторы
        //public FieldEmptiness(FieldCoordinates position)  ----- нужна ли тут позиция???
        public FieldEmptiness()
        {
            Figure = "*";  //  пока так, для отладки, потом будет пробел
            Color = ConsoleColor.Gray;
            BgColor = ConsoleColor.Black;
        }
        #endregion
    }
}
