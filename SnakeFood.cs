namespace SnakeGame
{
    internal class SnakeFood : IFieldCell
    {
        #region Поля
        #endregion

        #region Свойства
        //public FieldCoordinates Position { get; set; }
        public string Figure { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        //public FieldCell Cell { get; set; }
        #endregion

        #region Методы
        public void Consume(Snake.Snake snake, FieldCell cell)
        {
            cell.Value = snake.head;
            snake.RaiseSnake(cell);
            //  cell.UpdateCell(snake.head);  //  Срабатывает событие перерисовки ячейки.
        }
        #endregion

        #region Делегаты
        //public delegate void IFieldCellHandler(IFieldCell value);
        #endregion

        #region События
       // public event IFieldCell.IFieldCellHandler Changed;
        #endregion

        #region Конструкторы

        public SnakeFood(GameField field)
        {
            //Cell = field.Field[position.X, position.Y];
            //Position = position;
            Figure = "F";
            Color = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
        #endregion
    }
}
