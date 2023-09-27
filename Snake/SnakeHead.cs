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
        #endregion

        #region Свойства
        #endregion

        #region Методы
        public override void Move()
        {
            this.NewDirection = State.HeadDirection;
            base.Move();
        }

        public void ExploreNewCell()
        {

        }
        #endregion

        #region Конструкторы
        public SnakeHead(int x, int y) : base(x, y, State.HeadDirection)
        {
            this.NewDirection = State.HeadDirection;
        }
        #endregion
    }
}
