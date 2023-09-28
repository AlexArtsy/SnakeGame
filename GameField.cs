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

        private void InitGameField()
        {
            FillCellsWithEmptyness();
        }

        private void FillCellsWithEmptyness()
        {
            for (int x = 0; x < this.width; x += 1)
            {
                for (int y = 0; y < this.height; y += 1)
                {
                    var newCell = new FieldCell(x + this.X, y + this.Y, new FieldEmptiness());
                    //  newCell.Changed += State.rendering.UpdateFieldCell; перенесено в RenderProcessor
                    this.Field[x, y] = newCell;
                }
            }
        }

        public void GenerateFood()
        {
            Task foodTask = new Task(() =>
            {
                while (true)
                {
                    var x = RandomGen.GetRandomX(this.width);
                    var y = RandomGen.GetRandomY(this.height);
                    this.Field[x, y].Value = new SnakeFood(this, new FieldCoordinates(1, 1));
                    this.Field[x, y].UpdateCell(this.Field[x, y].Value);
                    Thread.Sleep(100 * new Random().Next(50, 100));
                }
                
            });
            foodTask.Start();
        }
        #endregion

        #region Конструкторы
        public GameField(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.width = width;
            this.height = height;
            this.Field = new FieldCell[this.width, this.height];

            InitGameField();
            GenerateFood();
        }
        #endregion
    }
}
