using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Field
{
    internal class SnakeFood : IFieldCellValue
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
        public void Consume(Snake snake, FieldCell cell)
        {
            //cell.Value = snake.head;    //  Выглядит не очень - переделать
            cell.IsBlinked = true;
            cell.BlinkColor = ConsoleColor.Green;
            snake.RaiseSnake(cell);
        }
        #endregion

        #region Делегаты
        //public delegate void IFieldCellHandler(IFieldCell value);
        #endregion

        #region События
        // public event IFieldCell.IFieldCellHandler Changed;
        #endregion

        #region Конструкторы

        //public SnakeFood(GameField field)
        //{
        //    Figure = "F";
        //    Color = ConsoleColor.White;
        //    BgColor = ConsoleColor.Black;
        //}

        public SnakeFood()
        {
            Figure = "F";
            Color = ConsoleColor.White;
            BgColor = ConsoleColor.Black;
        }
        #endregion
    }
}
