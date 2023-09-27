using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeMind
    {
        #region Поля
        private readonly List<SnakeMember> body;
        private readonly SnakeHead head;
        private readonly GameField field;
        #endregion

        #region Свойства
        #endregion

        #region Методы

        public void EatFood(SnakeFood food)
        {
            food.Consume(this.head);
        }

        public void CreateSnake()
        {
            var direction = this.head.Direction;

            var body1 = new SnakeBodyPart(GetNextBodiesPartPosition(this.head.Position, direction));
            var body2 = new SnakeBodyPart(GetNextBodiesPartPosition(body1.Position, direction));
            var body3 = new SnakeBodyPart(GetNextBodiesPartPosition(body2.Position, direction));
            var tail = new SnakeTail(GetNextBodiesPartPosition(body3.Position, direction));


            head.Figure = "1";
            body1.Figure = "2";
            body2.Figure = "3";
            body3.Figure = "4";
            tail.Figure = " ";

            head.Moved = this.field.
            body1.Figure = "2";
            body2.Figure = "3";
            body3.Figure = "4";
            tail.Figure = " ";

            this.body.Add(head);
            this.body.Add(body1);
            this.body.Add(body2);
            this.body.Add(body3);
            this.body.Add(tail);

        }
        public void CalculateNextHeadCoordinates(string direction)
        {
            var x = 0;
            var y = 0;
            var head = this.body[0];    //  Просто для красоты.

            switch (direction)
            {
                case "Up":
                    x = head.Position.X;
                    y = head.Position.Y == 0 ? this.field.height - 1 : head.Position.Y + 1;
                    break;
                case "Down":
                    x = head.Position.X;
                    y = head.Position.Y == this.field.height - 1 ? 0 : head.Position.Y - 1;
                    break;
                case "Right":
                    x = head.Position.X == this.field.width - 1 ? 0 : head.Position.X + 1;
                    y = head.Position.Y;
                    break;
                case "Left":
                    x = head.Position.X == 0 ? this.field.width - 1 : head.Position.X - 1;
                    y = head.Position.Y;
                    break;
            }

            head.NextPosition = new FieldCoordinates(x, y);
        }
        public void CalculateBodyMovingCoordinates()
        {
            for (var i = 0; i < this.body.Count - 1; i += 1)
            {
                this.body[i + 1].NextPosition = this.body[i].Position;
            }
        }

        public FieldCoordinates GetNextBodiesPartPosition(FieldCoordinates prevPosition, string direction)  //  Нужно только для инициализации змеи??
        {
            var x = 0;
            var y = 0;

            switch (direction)
            {
                case "Up":
                    x = prevPosition.X;
                    y = prevPosition.Y == 0 ? 1 : prevPosition.Y + 1;
                    break;
                case "Down":
                    x = prevPosition.X;
                    y = prevPosition.Y == 0 ? this.field.height : prevPosition.Y - 1;
                    break;
                case "Right":
                    x = x = prevPosition.X == this.field.width - 1 ? this.field.width - 2 : x = prevPosition.X - 1;
                    y = prevPosition.Y;
                    break;
                case "Left":
                    x = x = prevPosition.X == 0 ? 1 : x = prevPosition.X + 1;
                    y = prevPosition.Y;
                    break;
            }

            return new FieldCoordinates(x, y);
        }

        #endregion

        #region Конструкторы
        public SnakeMind(List<SnakeMember> body, SnakeHead head, GameField field)
        {
            this.body = body;
            this.head = head;    //  исключение, боди пустая
            this.field = field;
        }
        #endregion
    }
}
