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
        public string direction = "Up";
        #endregion

        #region Свойства
        public int X { get; set; }
        public int Y { get; set; }
        public ConsoleColor Color { get; set; }
        #endregion

        #region Методы

        public void MoveUp()
        {
            this.Y = this.Y == 0 ? Console.WindowHeight - 1 : this.Y - 1;
        }

        public void MoveDown()
        {
            this.Y = this.Y == Console.WindowHeight - 1 ? 0 : this.Y + 1;
        }

        public void MoveRight()
        {
            this.X = this.X == Console.WindowWidth ? 0 : this.X + 1;
        }

        public void MoveLeft()
        {
            this.X = this.X == 0 ? Console.WindowWidth - 1 : this.X - 1;
        }
        #endregion

        #region Делегаты и события
        public delegate void MoveDirection(); 
        public MoveDirection Move;

        #endregion
        #region Конструкторы
        public State()
        {
            this.X = 0;
            this.Y = 0;
        }
        #endregion
    }
}
