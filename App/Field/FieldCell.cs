
namespace SnakeGame.App.Field
{
    public class FieldCell
    {
        #region Поля
        private IFieldCellValue cellValue;
        #endregion

        #region Свойства
        public FieldCoordinates Position { get; set; }

        public IFieldCellValue Value
        {
            get => cellValue;
            set
            {
                cellValue = value;
                Changed?.Invoke(this);  //  Перерисовываем ячейку.
            }
        }

        //public ConsoleColor Color { get; set; }
        //public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы

        public void UpdateCell(IFieldCellValue value)    //  надо переписать чтобы событие срабатывало когда устанавливается Value ячейки
        {
            Value = value;
            //this.Color = value.Color;
            //this.BgColor = value.BgColor;

            Changed?.Invoke(this);
        }
        #endregion

        #region Делегаты
        public delegate void FieldCellHandler(FieldCell cell);
        #endregion

        #region События
        public event FieldCellHandler Changed;
        #endregion

        #region Конструкторы
        public FieldCell(int x, int y) //IFieldCell eny
        {
            Position = new FieldCoordinates(x, y);
            Value = new FieldEmptiness();
            //Color = ConsoleColor.Black;
            //BgColor = ConsoleColor.White;

            //Changed?.Invoke(this);
        }
        #endregion
    }
}
