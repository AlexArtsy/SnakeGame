

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
            for (int y = 0; y < height; y += 1)
            {
                for (int x = 0; x < width; x += 1)
                {
                    var newCell = new FieldCell(x + X, y + Y);
                    Field[x, y] = newCell;
                }
            }
        }

        private void SetBorders()
        {
            for (int x = 0; x < width; x += 1)
            {
                Field[x, 0].Value = new FieldWall();
                Field[x, height - 1].Value = new FieldWall();
            }

            for (int y = 1; y < height - 1; y += 1)
            {
                Field[0, y].Value = new FieldWall();
                Field[width - 1, y].Value = new FieldWall();
            }
        }

        public void GenerateFood()
        {
            while (State.IsSnakeAlive)
            {
                var x = RandomGen.GetRandomX(width);
                var y = RandomGen.GetRandomY(height);

                if (Field[x, y].Value.ToString() != "SnakeGame.FieldEmptiness")
                {
                    GenerateFood();
                }

                if (State.FoodPiecesValue <= 5)
                {
                    Field[x, y].Value = new SnakeFood(this);
                    State.FoodPiecesValue += 1;
                }
                //Field[x, y].UpdateCell(Field[x, y].Value);
                Thread.Sleep(100 * new Random().Next(50, 1000 - State.SnakeSpeed));
            }
        }
        #endregion

        #region Конструкторы
        public GameField(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            this.width = width;
            this.height = height;
            Field = new FieldCell[this.width, this.height];

            InitGameField();
            SetBorders();
            RenderProcessor.SubscribeFieldCellChangingEvent(this);
            //GenerateFood();
        }
        #endregion
    }
}
