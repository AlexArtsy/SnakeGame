using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeTail : SnakeMember
    {
        #region Поля
        #endregion

        #region Свойства
        #endregion

        #region Методы

        
        #endregion

        #region Конструкторы
        public SnakeTail(int prevX, int prevY, State state, string direction) : base(prevX, prevY, state, direction)
        {
            this.Color = Console.ForegroundColor;
            this.BgColor = Console.BackgroundColor;
            this.Figure = "_";
        }
        #endregion
    }
}
