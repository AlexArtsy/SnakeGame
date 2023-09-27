using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Snake;

namespace SnakeGame
{
    internal class GameField
    {
        #region Поля
        public int width;
        public int height;
        #endregion

        #region Свойства
        public int X { get; set; }
        public int Y { get; set; }
        public FieldCell[,] Field { get; set; }
        #endregion

        #region Методы
        //private void PassDirectionToNext(Snake.Snake snake)
        //{
        //    for (int i = 0; i < snake.Parts.Count - 1; i += 1)
        //    {
        //        snake.Parts[i + 1].NewDirection = snake.Parts[i].CurrentDirection;
        //    }
        //}
        ////private void MoveAllParts(Snake.Snake snake)
        ////{
        ////    snake.Parts.ForEach(p => p.Move());
        ////}

        //private void SetCurrentDirection(Snake.Snake snake)
        //{
        //    snake.Parts.ForEach(p => p.CurrentDirection = p.NewDirection);
        //}

        //public void MoveSnakePartUp(SnakeMember part)
        //{
        //    part.Y = part.Y == 0 ? this.height - 1 : part.Y - 1;
        //}

        //public void MoveSnakePartDown(SnakeMember part)
        //{
        //    part.Y = part.Y == this.height - 1 ? 0 : part.Y + 1;
        //}

        //public void MoveSnakePartRight(SnakeMember part)
        //{
        //    part.X = part.X == this.width - 1 ? 0 : part.X + 1;
        //}

        //public void MoveSnakePartLeft(SnakeMember part)
        //{
        //    part.X = part.X == 0 ? this.width - 1 : part.X - 1;
        //}

        //public void MoveSnake(Snake.Snake snake)
        //{
        //    PassDirectionToNext(snake);

        //    snake.Parts.ForEach(p =>
        //    {
        //        switch (p.NewDirection)
        //        {
        //            case "Up":
        //                MoveSnakePartUp(p);
        //                break;
        //            case "Down":
        //                MoveSnakePartDown(p);
        //                break;
        //            case "Right":
        //                MoveSnakePartRight(p);
        //                break;
        //            case "Left":
        //                MoveSnakePartLeft(p);
        //                break;
        //        }
        //        //p.Move();   //  Событие для отрисовки.
        //    });

        //    SetCurrentDirection(snake);
        //    UpdateFieldState(snake);
        //}

        private void UpdateFieldState(Snake.Snake snake)
        {
            snake.Parts.ForEach(p =>
            {
                this.Field[p.X, p.Y].UpdateCell(p.Figure, p.Color, p.BgColor);
            });
        }

        private void InitGameField()
        {
            this.Field = new FieldCell[this.width, this.height];
            var startValue = " ";
            for (int x = 0; x < this.width; x += 1)
            {
                for (int y = 0; y < this.height; y += 1)
                {
                    var newCell = new FieldCell(x + this.X, y + this.Y, startValue);
                    newCell.Changed += State.rendering.UpdateFieldCell;
                    this.Field[x, y] = newCell;
                }
            }
        }
        #endregion

        #region Конструкторы
        public GameField(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.width = width;
            this.height = height;

            InitGameField();
        }
        #endregion
    }
}
