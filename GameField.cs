using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class GameField
    {
        #region Поля
        private int width;
        private int height;
        #endregion

        #region Свойства
        #endregion

        #region Методы
        #endregion

        #region Конструкторы
        public GameField(State state)
        {
            state.fieldWidth = width;
            state.fieldHeight = height;
        }
        #endregion
    }
}
