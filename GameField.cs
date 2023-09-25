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
        #endregion

        #region Свойства
        public int Width { get; set; }
        public int Height { get; set; }
        #endregion

        #region Методы
        #endregion

        #region Конструкторы
        public GameField(int width = 50, int height = 50)
        {
            this.Width = width;
            this.Height = height;
        }
        #endregion
    }
}
