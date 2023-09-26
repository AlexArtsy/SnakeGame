using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeHead : SnakeMember
    {
        #region Поля

        private readonly State state;
        #endregion

        #region Свойства
        #endregion

        #region Методы
        public override void Move()
        {
            this.NewDirection = state.HeadDirection;
            base.Move();
        }
        #endregion

        #region Конструкторы
        public SnakeHead(int x, int y, State state) : base(x, y, state, state.HeadDirection)
        {
            this.state = state;
            this.NewDirection = state.HeadDirection;
        }
        #endregion
    }
}
