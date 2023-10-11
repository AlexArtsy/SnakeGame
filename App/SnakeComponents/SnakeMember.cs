
using SnakeGame.App.Field;

namespace SnakeGame.App.SnakeComponents
{
    public class SnakeMember : IFieldCellValue, ISnakeable
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
            field.Field[Position.X, Position.Y].Value = this; //  Происходит перерисовка
        }

        private void UpdateCurrentPosition()
        {
            Position = NextPosition;
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
            Position = position;
            Figure = "O";
            Color = ConsoleColor.Black;
            BgColor = ConsoleColor.White;
        }

        public SnakeMember()
        {
            Figure = "O";
            Color = ConsoleColor.Black;
            BgColor = ConsoleColor.White;
        }
        #endregion
    }
}
