namespace SnakeGame
{
    internal class SnakeFood : IFieldCell
    {
        #region Поля
        #endregion

        #region Свойства
        public FieldCoordinates Position { get; set; }
        public string Figure { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        public FieldCell Cell { get; set; }
        #endregion

        #region Методы
        public void Consume(IFieldCell head)
        {
            Cell.UpdateCell(head);  //  Срабатывает событие перерисовки ячейки.
        }
        #endregion

        #region Конструкторы

        public SnakeFood(GameField field, FieldCoordinates position)
        {
            Cell = field.Field[position.X, position.Y];
            Position = position;
            Figure = "F";
            Color = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
        #endregion
    }
}
