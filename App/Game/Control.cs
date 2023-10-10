using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.Game
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

        public static void IncreaseSpeed()
        {
            State.Score += 100;
        }

        public static void DecreaseFoodValue()
        {
            State.FoodPiecesValue -= 1;
        }

        public static void DecreaseScore()
        {
            State.Score -= 1;
        }

        public static void KillSnake()
        {
            State.IsSnakeAlive = false;
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
