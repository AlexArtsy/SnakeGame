using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class Control
    {
        #region Поля
        private readonly State state;
        #endregion

        #region Свойства
        #endregion

        #region Методы

        #endregion

        #region Делегаты и события
        public delegate void MoveDirection();
        public MoveDirection Move;
        #endregion

        #region Конструкторы

        public Control(State state)
        {
            this.state = state;
        }
        #endregion
    }
}
