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

        public FieldCell ExploreNextCell()
        {
            var nextCell = SeeNextCell();

            Console.SetCursorPosition(0,0);
            Console.Write("                                                           ");
            Console.SetCursorPosition(0, 0);
            Console.Write($"Содержимое ячейки: {nextCell.Value.GetType()}");

            return nextCell;
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

            //head.Moved = this.field.
            //body1.Figure = "2";
            //body2.Figure = "3";
            //body3.Figure = "4";
            //tail.Figure = " ";

            this.body.Add(head);
            this.body.Add(body1);
            this.body.Add(body2);
            this.body.Add(body3);
            this.body.Add(tail);

        }

        public FieldCell SeeNextCell()
        {
            var nextPosition = GetNextPosition(this.head.Position, this.head.Direction);
            return this.field.Field[nextPosition.X, nextPosition.Y];
        }

        public FieldCoordinates GetNextPosition(FieldCoordinates current, string direction)
        {
            var x = 0;
            var y = 0;

            switch (direction)
            {
                case "Up":
                    x = current.X;
                    y = current.Y == 0 ? this.field.height - 1 : current.Y - 1;
                    break;
                case "Down":
                    x = current.X;
                    y = current.Y == this.field.height - 1 ? 0 : current.Y + 1;
                    break;
                case "Right":
                    x = current.X == this.field.width - 1 ? 0 : current.X + 1;
                    y = current.Y;
                    break;
                case "Left":
                    x = current.X == 0 ? this.field.width - 1 : current.X - 1;
                    y = current.Y;
                    break;
            }

            return new FieldCoordinates(x, y);
        }
        public void SetNextHeadCoordinates(string direction)
        {
            this.head.NextPosition = GetNextPosition(this.head.Position, direction);
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
