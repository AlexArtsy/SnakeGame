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

        public void DirectionListener()
        {
            while (true)
            {
                var pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Tab:
                        break;
                    case ConsoleKey.LeftArrow:
                        State.HeadDirection = State.HeadDirection == "Right" ? "Right" : "Left";
                        break;
                    case ConsoleKey.RightArrow:
                        State.HeadDirection = State.HeadDirection == "Left" ? "Left" : "Right";
                        break;
                    case ConsoleKey.UpArrow:
                        State.HeadDirection = State.HeadDirection == "Down" ? "Down" : "Up";
                        break;
                    case ConsoleKey.DownArrow:
                        State.HeadDirection = State.HeadDirection == "Up" ? "Up" : "Down";
                        break;
                    //case ConsoleKey.PageUp:
                    //    State.SnakeSpeed = State.SnakeSpeed == 0 ? 0 : State.SnakeSpeed - 20;
                    //    break;
                    //case ConsoleKey.PageDown:
                    //    State.SnakeSpeed += 20;
                        //break;
                    case ConsoleKey.Backspace:
                        break;
                    default:
                        break;
                }
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
