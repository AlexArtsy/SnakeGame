using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeMember : IMovable, IFieldCellValue, ISnakeable
    {
        #region Поля
        private ConsoleColor color;
        private ConsoleColor bgColor;
        #endregion

        #region Свойства
        public FieldCoordinates Position { get; set; }
        public FieldCoordinates NextPosition { get; set; }
        public string Figure { get; set; }
        public int Speed { get; set; }

        public ConsoleColor Color { get; set; }

        public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы
        #region Moving function
        public virtual void Move(GameField field)
        {
            UpdateCurrentPosition();
            field.Field[this.Position.X, this.Position.Y].Value = this; //  Происходит перерисовка
        }

        private void UpdateCurrentPosition()
        {
            this.Position = this.NextPosition;
        }

        public void Consume(Snake snake, FieldCell cell)
        {
            snake.Die(cell);
        }
        #endregion

        #endregion

        #region Делегаты
        public delegate void SnakeMemberHandler(SnakeMember part);
       // public delegate void IFieldCellHandler(IFieldCell value);
        #endregion

        #region События
        //public event SnakeMemberHandler Moved;
        //public event IFieldCell.IFieldCellHandler Changed;
        #endregion

        #region Конструкторы
        public SnakeMember(FieldCoordinates position)
        {
            this.Position = position;
            Figure = "O";
            Color = ConsoleColor.Black;
            BgColor = ConsoleColor.White;
        }
        #endregion
    }
}
