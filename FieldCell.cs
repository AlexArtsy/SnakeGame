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
        public FieldCoordinates Position { get; set; }
        public IFieldCell Value { get; set; }
        //public ConsoleColor Color { get; set; }
        //public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы

        public void UpdateCell(IFieldCell value)    //  надо переписать чтобы событие срабатывало когда устанавливается Value ячейки
        {
            this.Value = value;
            //this.Color = value.Color;
            //this.BgColor = value.BgColor;

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
        public FieldCell(int x, int y, IFieldCell eny)
        {
            this.Position = new FieldCoordinates(x, y);
            this.Value = eny;
            //Color = ConsoleColor.Black;
            //BgColor = ConsoleColor.White;

            Changed?.Invoke(this);
        }
        #endregion
    }
}
