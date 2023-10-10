using SnakeGame.App.Field;

namespace SnakeGame.App.SnakeComponents
{
    public class SnakeHead : SnakeMember, ISnakeable
    {
        #region Поля
        #endregion

        #region Свойства
        public string Direction { get; set; }
        #endregion

        #region Методы

        public void Eat(IFieldCellValue food, FieldCell cell, Snake snake)
        {
            //  Проверить порядок выполнения
            food.Consume(snake, cell);
            cell.Value = this;
        }
        #endregion

        #region Конструкторы
        public SnakeHead(FieldCoordinates position, string direction) : base(position)
        {
            Direction = direction;
        }
        #endregion
    }
}
