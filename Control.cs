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
        #endregion

        #region Свойства
        #endregion

        #region Методы

        public static void DirectionListener(string direction)
        {
            switch (direction)
            {
                case "Left":
                    State.HeadDirection = State.HeadDirection == "Right" ? "Right" : "Left";
                    break;
                case "Right":
                    State.HeadDirection = State.HeadDirection == "Left" ? "Left" : "Right";
                    break;
                case "Up":
                    State.HeadDirection = State.HeadDirection == "Down" ? "Down" : "Up";
                    break;
                case "Down":
                    State.HeadDirection = State.HeadDirection == "Up" ? "Up" : "Down";
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Делегаты и события
        public delegate void MoveDirection();
        public MoveDirection Move;
        #endregion

        #region Конструкторы

        public Control()
        {

        }
        #endregion
    }
}
