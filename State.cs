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
        public RenderProcessor rendering = new RenderProcessor();
        #endregion

        #region Свойства
        public string HeadDirection { get; set; }
        #endregion

        #region Методы

        
        #endregion

        #region Конструкторы
        public State(int width, int height, int speed)
        {
            this.fieldWidth = width;
            this.fieldHeight = height;
            this.speed = speed;
            this.HeadDirection = "Right";
        }
        #endregion
    }
}
