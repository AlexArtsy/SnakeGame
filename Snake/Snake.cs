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
        private readonly State state;
        #endregion

        #region Свойства
        public List<SnakeMember> Parts { get; set; }

        #endregion

        #region Методы
        public void Move()
        {
            PassDirectionToNext();
            MoveAllParts();
            SetCurrentDirection();
        }

        private void PassDirectionToNext()
        {
            for (int i = 0; i < this.Parts.Count - 1; i += 1)
            {
                this.Parts[i + 1].NewDirection = this.Parts[i].CurrentDirection;
            }
        }
        private void MoveAllParts()
        {
            this.Parts.ForEach(p => p.Move());
        }

        private void SetCurrentDirection()
        {
            this.Parts.ForEach(p => p.CurrentDirection = p.NewDirection);
        }
        #endregion
        private void CreateSnake(int x, int y, State state)
        {
            var head = new SnakeHead(x, y, state);
            var body1 = new SnakeBodyPart(GetNextX(head.X, state.HeadDirection), GetNextY(head.Y, state.HeadDirection), state, state.HeadDirection);
            var body2 = new SnakeBodyPart(GetNextX(body1.X, state.HeadDirection), GetNextY(body1.Y, state.HeadDirection), state, state.HeadDirection);
            var body3 = new SnakeBodyPart(GetNextX(body2.X, state.HeadDirection), GetNextY(body2.Y, state.HeadDirection), state, state.HeadDirection);
            var tail = new SnakeTail(GetNextX(body3.X, state.HeadDirection), GetNextY(body3.Y, state.HeadDirection), state, state.HeadDirection);


            head.Moved += state.rendering.RenderSnakeMember;
            body1.Moved += state.rendering.RenderSnakeMember;
            body2.Moved += state.rendering.RenderSnakeMember;
            body3.Moved += state.rendering.RenderSnakeMember;
            tail.Moved += state.rendering.RenderSnakeMember;

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
                    x = prevX == state.fieldWidth ? state.fieldWidth - 1 : prevX - 1;
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
                    y = prevY == 0 ? state.fieldHeight : prevY - 1;
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

        public Snake(int x, int y, State state)
        {
            this.state = state;
            this.Parts = new List<SnakeMember>();
            CreateSnake(x, y, state);
        }

        #endregion
    }
}
