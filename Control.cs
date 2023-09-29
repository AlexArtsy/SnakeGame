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

        public void KeyEventListener()
        {
            while (true)
            {
                var pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Tab:
                        break;
                    case ConsoleKey.LeftArrow:
                        State.HeadDirection = "Left";
                        break;
                    case ConsoleKey.RightArrow:
                        State.HeadDirection = "Right";
                        break;
                    case ConsoleKey.UpArrow:
                        State.HeadDirection = "Up";
                        break;
                    case ConsoleKey.DownArrow:
                        State.HeadDirection = "Down";
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
