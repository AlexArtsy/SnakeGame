using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class SnakeHead : SnakeMember
    {
        #region Поля
        #endregion

        #region Свойства
        #endregion

        #region Методы
        #endregion

        #region Конструкторы
        public SnakeHead(int x, int y, State state) : base(x, y, state, state.HeaDirection)
        {

        }
        #endregion
    }
}
