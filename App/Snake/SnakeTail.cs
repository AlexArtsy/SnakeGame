using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Field;

namespace SnakeGame.App.Snake
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
            Position = NextPosition;
        }

        private void CaptureCell(GameField field)
        {
            field.Field[Position.X, Position.Y].UpdateCell(new FieldEmptiness()); //  Передаем в ячейку пустоту вместо себя.
        }

        #endregion

        #region Конструкторы
        public SnakeTail(FieldCoordinates position) : base(position)
        {
            Color = Console.ForegroundColor;
            BgColor = Console.BackgroundColor;
            Figure = " ";
        }
        #endregion
    }
}
