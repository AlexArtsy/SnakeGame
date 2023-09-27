using SnakeGame.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class FieldCell
    {
        #region Поля

        #endregion

        #region Свойства
        public int X { get; set; }  //  Внешний Х
        public int Y { get; set; }  //   Внешний Y
        public string Value { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы

        public void UpdateCell(string value, ConsoleColor color, ConsoleColor bg)
        {
            this.Color = color;
            this.BgColor = bg;
            this.Value = value;
            Changed?.Invoke(this);
        }
        #endregion
        #region Делегаты
        public delegate void FieldCellHandler(FieldCell cell);
        #endregion

        #region События
        public event FieldCellHandler Changed;
        #endregion

        #region Конструкторы
        public FieldCell(int x, int y, string value)
        {
            this.X = x;
            this.Y = y;
            this.Value = value;
            Color = ConsoleColor.Black;
            BgColor = ConsoleColor.White;
        }
        #endregion
    }
}
