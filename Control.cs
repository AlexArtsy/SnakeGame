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
        public void MoveUp()
        {
            state.Y = state.Y == 0 ? Console.WindowHeight - 1 : state.Y - 1;
        }

        public void MoveDown()
        {
            state.Y = state.Y == Console.WindowHeight - 1 ? 0 : state.Y + 1;
        }

        public void MoveRight()
        {
            state.X = state.X == Console.WindowWidth ? 0 : state.X + 1;
        }

        public void MoveLeft()
        {
            state.X = state.X == 0 ? Console.WindowWidth - 1 : state.X - 1;
        }

        public void KeyListener()
        {
            var pressedKey = Console.ReadKey(true);
            switch (pressedKey.Key)
            {
                case ConsoleKey.Tab:
                    break;
                case ConsoleKey.LeftArrow:
                    Move = MoveLeft;
                    break;
                case ConsoleKey.RightArrow:
                    Move = MoveRight;
                    break;
                case ConsoleKey.UpArrow:
                    Move = MoveUp;
                    break;
                case ConsoleKey.DownArrow:
                    Move = MoveDown;
                    break;
                case ConsoleKey.Enter:
                    break;
                case ConsoleKey.Backspace:
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

        public Control(State state)
        {
            this.state = state;
        }
        #endregion
    }
}
