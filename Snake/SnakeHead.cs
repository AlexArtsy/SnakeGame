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
        public override void Move(GameField field)
        {
            this.Direction = State.HeadDirection;
            this.Speed = State.SnakeSpeed;
            base.Move(field);
        }
        public void EatFood(SnakeFood food)
        {
            food.Consume(this);
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
