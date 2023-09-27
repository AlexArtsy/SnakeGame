using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class Snake
    {
        #region Поля
        //private readonly State state;
        private readonly GameField gameField;
        #endregion

        #region Свойства
        public int Speed { get; set; }
        public string HeadDirection { get; set; }
        public List<SnakeMember> Parts { get; set; }

        #endregion

        #region Методы
        public void Move()
        {
            this.gameField.MoveSnake(this);
        }
        private void PassDirectionToNext(Snake.Snake snake)
        {
            for (int i = 0; i < snake.Parts.Count - 1; i += 1)
            {
                snake.Parts[i + 1].NewDirection = snake.Parts[i].CurrentDirection;
            }
        }

        private void SetCurrentDirection(Snake.Snake snake)
        {
            snake.Parts.ForEach(p => p.CurrentDirection = p.NewDirection);
        }

        public void MoveSnakePartUp(SnakeMember part)
        {
            part.Y = part.Y == 0 ? this.height - 1 : part.Y - 1;
        }

        public void MoveSnakePartDown(SnakeMember part)
        {
            part.Y = part.Y == this.height - 1 ? 0 : part.Y + 1;
        }

        public void MoveSnakePartRight(SnakeMember part)
        {
            part.X = part.X == this.width - 1 ? 0 : part.X + 1;
        }

        public void MoveSnakePartLeft(SnakeMember part)
        {
            part.X = part.X == 0 ? this.width - 1 : part.X - 1;
        }

        public void MoveSnake(Snake.Snake snake)
        {
            PassDirectionToNext(snake);

            snake.Parts.ForEach(p =>
            {
                switch (p.NewDirection)
                {
                    case "Up":
                        MoveSnakePartUp(p);
                        break;
                    case "Down":
                        MoveSnakePartDown(p);
                        break;
                    case "Right":
                        MoveSnakePartRight(p);
                        break;
                    case "Left":
                        MoveSnakePartLeft(p);
                        break;
                }
                //p.Move();   //  Событие для отрисовки.
            });

            SetCurrentDirection(snake);
            UpdateFieldState(snake);
        }


        

        #endregion
        private void CreateSnake(int x, int y)
        {
            var head = new SnakeHead(x, y);
            var body1 = new SnakeBodyPart(GetNextX(head.X, this.HeadDirection), GetNextY(head.Y, this.HeadDirection), this.HeadDirection);
            var body2 = new SnakeBodyPart(GetNextX(body1.X, this.HeadDirection), GetNextY(body1.Y, this.HeadDirection), this.HeadDirection);
            var body3 = new SnakeBodyPart(GetNextX(body2.X, this.HeadDirection), GetNextY(body2.Y, this.HeadDirection), this.HeadDirection);
            var tail = new SnakeTail(GetNextX(body3.X, this.HeadDirection), GetNextY(body3.Y, this.HeadDirection), this.HeadDirection);


            //head.Moved += state.rendering.RenderSnakeMember;
            //body1.Moved += state.rendering.RenderSnakeMember;
            //body2.Moved += state.rendering.RenderSnakeMember;
            //body3.Moved += state.rendering.RenderSnakeMember;
            //tail.Moved += state.rendering.RenderSnakeMember;

            head.Figure = "1";
            body1.Figure = "2";
            body2.Figure = "3";
            body3.Figure = "4";
            tail.Figure = " ";

            this.Parts.Add(head);
            this.Parts.Add(body1);
            this.Parts.Add(body2);
            this.Parts.Add(body3);
            this.Parts.Add(tail);
            
        }

        private int GetNextX(int prevX, string direction)
        {
            var x = prevX;
            switch (direction)
            {
                case "Up":
                    x = prevX;
                    break;
                case "Down":
                    x = prevX;
                    break;
                case "Right":
                    x = prevX == this.gameField.width - 1 ? this.gameField.width - 2 : prevX - 1;
                    break;
                case "Left":
                    x = prevX == 0 ? 1 : prevX + 1;
                    break;
            }
            return x;
        }
        private int GetNextY(int prevY, string direction)
        {
            var y = prevY;
            switch (direction)
            {
                case "Up":
                    y = prevY == 0 ? 1 : prevY + 1;
                    break;
                case "Down":
                    y = prevY == 0 ? this.gameField.height : prevY - 1;
                    break;
                case "Right":
                    y = prevY;
                    break;
                case "Left":
                    y = prevY;
                    break;
            }
            return y;
        }

        #region Конструкторы

        public Snake(int x, int y, GameField gameField, int speed)
        {
            this.Parts = new List<SnakeMember>();
            this.gameField = gameField;
            this.Speed = speed;
            this.HeadDirection = "Right";
            CreateSnake(x, y);
        }

        #endregion
    }
}
