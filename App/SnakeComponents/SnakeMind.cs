﻿using SnakeGame.App.Field;
using SnakeGame.App.GameComponents;

namespace SnakeGame.App.SnakeComponents
{
    public class SnakeMind
    {
        #region Поля
        private readonly List<SnakeMember> body;
        private readonly SnakeHead head;
        private readonly GameField field;
        #endregion

        #region Свойства
        public State State { get; set; }
        #endregion

        #region Методы

        public void ReadDirection()
        {
            head.Direction = CheckDirectionCorrectness(this.head.Direction, this.State.HeadDirection)
                ? this.State.HeadDirection
                : this.head.Direction;
        }

        public void SetSpeed()
        {
            head.Speed = State.SnakeSpeed;
        }

        public FieldCell ExploreNextCell()
        {
            var nextPosition = GetNextPosition(head.Position, head.Direction);
            var nextCell = field.Field[nextPosition.X, nextPosition.Y];

            return nextCell;
        }

        public void CreateSnake()
        {
            var direction = head.Direction;

            var body1 = new SnakeBodyPart(GetNextBodiesPartPosition(head.Position, direction));
            //var body2 = new SnakeBodyPart(GetNextBodiesPartPosition(body1.Position, direction));
            //var body3 = new SnakeBodyPart(GetNextBodiesPartPosition(body2.Position, direction));
            var tail = new SnakeTail(GetNextBodiesPartPosition(body1.Position, direction));


            head.Figure = "1";
            body1.Figure = "2";
            //body2.Figure = "3";
            //body3.Figure = "4";
            tail.Figure = " ";


            body.Add(head);
            body.Add(body1);
            //body.Add(body2);
            //body.Add(body3);
            body.Add(tail);

            State.IsSnakeAlive = true;

        }

        private bool CheckDirectionCorrectness(string currentDirection, string nextDirection)
        {
            var isCorrect = false;
            switch (nextDirection)
            {
                case "Up":
                    isCorrect = currentDirection != "Down";
                    break;
                case "Down":
                    isCorrect = currentDirection != "Up";
                    break;
                case "Right":
                    isCorrect = currentDirection != "Left";
                    break;
                case "Left":
                    isCorrect = currentDirection != "Right";
                    break;
            }

            return isCorrect;
        }

        public FieldCoordinates GetNextPosition(FieldCoordinates current, string direction)
        {
            var x = 0;
            var y = 0;

            switch (direction)
            {
                case "Up":
                    x = current.X;
                    y = current.Y == 0 ? field.height - 1 : current.Y - 1;
                    break;
                case "Down":
                    x = current.X;
                    y = current.Y == field.height - 1 ? 0 : current.Y + 1;
                    break;
                case "Right":
                    x = current.X == field.width - 1 ? 0 : current.X + 1;
                    y = current.Y;
                    break;
                case "Left":
                    x = current.X == 0 ? field.width - 1 : current.X - 1;
                    y = current.Y;
                    break;
            }

            return new FieldCoordinates(x, y);
        }
        public void SetNextHeadCoordinates(string direction)
        {
            head.NextPosition = GetNextPosition(head.Position, direction);
        }
        public void CalculateBodyMovingCoordinates()
        {
            for (var i = 0; i < body.Count - 1; i += 1)
            {
                body[i + 1].NextPosition = body[i].Position;
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
                    y = prevPosition.Y == 0 ? field.height : prevPosition.Y - 1;
                    break;
                case "Right":
                    x = x = prevPosition.X == field.width - 1 ? field.width - 2 : x = prevPosition.X - 1;
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
        public SnakeMind(List<SnakeMember> body, SnakeHead head, GameField field, State state)
        {
            this.body = body;
            this.head = head;    //  исключение, боди пустая
            this.field = field;
            this.State = state;
        }
        #endregion
    }
}
