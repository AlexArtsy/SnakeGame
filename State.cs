using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class State
    {
        #region Поля
        public int fieldHeight;
        public int fieldWidth;
        public int speed;
        #endregion

        #region Свойства
        public Direction HeaDirection { get; set; }
        #endregion

        #region Методы

        
        #endregion

        #region Конструкторы
        public State(string direction)
        {
            this.HeaDirection = new Direction(direction);
        }
        #endregion
    }
}
