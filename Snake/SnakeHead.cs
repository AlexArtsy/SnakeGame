using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeHead : SnakeMember, ISnakeable
    {
        #region Поля
        #endregion

        #region Свойства
        public string Direction { get; set; }
        #endregion

        #region Методы
        public override void Move()
        {
            this.Direction = State.HeadDirection;
            base.Move();
        }

        public void ExploreNextCell(FieldCell cell)
        {

        }
        #endregion

        #region Конструкторы
        public SnakeHead(FieldCoordinates position, string direction) : base(position)
        {
            this.Direction = direction;
        }
        #endregion
    }
}
