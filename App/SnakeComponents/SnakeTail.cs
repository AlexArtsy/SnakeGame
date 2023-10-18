using SnakeGame.App.Field;

namespace SnakeGame.App.SnakeComponents
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
            //field.Field[Position.X, Position.Y].UpdateCell(new FieldEmptiness()); //  Передаем в ячейку пустоту вместо себя.
            field.Field[Position.X, Position.Y].Value = new FieldEmptiness();
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
