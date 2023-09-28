using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeTail : SnakeMember, ISnakeable
    {
        #region Поля
        #endregion

        #region Свойства
        #endregion

        #region Методы

        public override void Move(GameField field)
        {
            UpdateCurrentPosition();
            CaptureCell(field);
        }

        private void UpdateCurrentPosition()
        {
            this.Position = this.NextPosition;
        }

        private void CaptureCell(GameField field)
        {
            field.Field[this.Position.X, this.Position.Y].UpdateCell(new FieldEmptiness()); //  Передаем в ячейку пустоту вместо себя.
        }

        #endregion

        #region Конструкторы
        public SnakeTail(FieldCoordinates position) : base(position)
        {
            this.Color = Console.ForegroundColor;
            this.BgColor = Console.BackgroundColor;
            this.Figure = " ";
        }
        #endregion
    }
}
