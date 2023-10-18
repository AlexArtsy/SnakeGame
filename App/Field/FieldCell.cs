

using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Field
{
    public class FieldCell
    {
        #region Поля
        private IFieldCellValue cellValue;
        #endregion

        #region Свойства
        public FieldCoordinates Position { get; set; }
        public bool IsChanged { get; set; }
        public bool IsBlinked { get; set; }
        public ConsoleColor BlinkColor { get; set; }

        public IFieldCellValue Value
        {
            get => cellValue;
            set
            {
                cellValue = value;
                //Changed?.Invoke(this);  //  Перерисовываем ячейку.
                this.IsChanged = true;

                if (value.Equals(new FieldWall()))
                {
                    this.IsBlinked = true;
                    this.BlinkColor = ConsoleColor.Red;
                }

                if (value.Equals(new SnakeFood()))
                {
                    this.IsBlinked = true;
                    this.BlinkColor = ConsoleColor.Green;
                }
            }
        }

        //public ConsoleColor Color { get; set; }
        //public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы

        //public void UpdateCell(IFieldCellValue value)    //  надо переписать чтобы событие срабатывало когда устанавливается Value ячейки
        //{
        //    Value = value;
        //    //this.Color = value.Color;
        //    //this.BgColor = value.BgColor;

        //    Changed?.Invoke(this);
        //}
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
