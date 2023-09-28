using SnakeGame.Snake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class FieldCoordinates
    {
        #region Поля

        #endregion

        #region Свойства
        public int X { get; set; }
        public int Y { get; set; }
        #endregion

        #region Методы
        #endregion

        #region Конструкторы
        public FieldCoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }
        #endregion
    }
}
