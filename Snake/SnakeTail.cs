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
