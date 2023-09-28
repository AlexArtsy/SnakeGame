using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeMember : IMovable, IFieldCell, ISnakeable
    {
        #region Поля
        //private readonly State state;
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
            CaptureCell(field);
            //Moved?.Invoke(this);    //  а это как-то обрабатывается???
        }

        private void UpdateCurrentPosition()
        {
            this.Position = this.NextPosition;
        }

        private void CaptureCell(GameField field)
        {
            field.Field[this.Position.X, this.Position.Y].UpdateCell(this);
        }
        #endregion

        #endregion

        #region Делегаты
        public delegate void SnakeMemberHandler(SnakeMember part);
        #endregion

        #region События
        public event SnakeMemberHandler Moved;
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
