using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeMember : IMovable
    {
        #region Поля
        private readonly State state;
        #endregion

        #region Свойства
        public int X { get; set; }
        public int Y { get; set; }
        public string Figure { get; set; }
        //public Direction Direction { get; set; }
        public string CurrentDirection { get; set; }
        public string NewDirection { get; set; }
        public int Speed { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы
        #region Moving function
        public void MoveUp()
        {
            Y = Y == 0 ? state.fieldHeight - 1 : Y - 1;
        }

        public void MoveDown()
        {
            Y = Y == state.fieldHeight - 1 ? 0 : Y + 1;
        }

        public void MoveRight()
        {
            X = X == state.fieldWidth ? 0 : X + 1;
        }

        public void MoveLeft()
        {
            X = X == 0 ? state.fieldWidth - 1 : X - 1;
        }
        public virtual void Move()
        {
            switch (this.NewDirection)
            {
                case "Up":
                    MoveUp();
                    break;
                case "Down":
                    MoveDown();
                    break;
                case "Right":
                    MoveRight();
                    break;
                case "Left":
                    MoveLeft();
                    break;
            }
            Moved?.Invoke(this);
        }
        #endregion

        #endregion

        #region Делегаты
        public delegate void SnakeMemberHandler(SnakeMember part);
        #endregion

        #region События
        public event SnakeMemberHandler Moved;
        #endregion

        #region Конструкторы
        public SnakeMember(int x, int y, State state, string direction)
        {
            this.X = x;
            this.Y = y;
            Figure = "O";
            Color = ConsoleColor.Black;
            BgColor = ConsoleColor.White;
            this.NewDirection = direction;
            this.CurrentDirection = direction;
            this.state = state;
        }
        #endregion
    }
}
