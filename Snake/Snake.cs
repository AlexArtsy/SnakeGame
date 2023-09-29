
namespace SnakeGame.Snake
{
    internal class Snake
    {
        #region Поля
        private readonly GameField gameField;
        public SnakeHead head;
        #endregion

        #region Свойства
        public int Speed { get; set; }
        //  public string HeadDirection { get; set; }
        public SnakeMind Mind { get; set; }
        public List<SnakeMember> Body { get; set; }

        #endregion

        #region Методы

        public void RunSnake()
        {
            while (true)
            {
                Move();

                var next = this.Mind.ExploreNextCell();

                if (next.Value.ToString() == "SnakeGame.SnakeFood")
                {
                    RaiseSnake(next);
                }

                //Thread.Sleep(1000 - State.SnakeSpeed);
            }
        }
        public void Move()
        {
            Thread.Sleep(1000 - State.SnakeSpeed);
            this.Mind.SetNextHeadCoordinates(this.head.Direction);
            this.Mind.CalculateBodyMovingCoordinates();

            this.Body.ForEach(p =>
            {
                p.Move(this.gameField);
            });

        }

        public void RaiseSnake(FieldCell cell)
        {
            this.Body.Insert(1, new SnakeBodyPart(this.head.Position));
            this.Mind.SetNextHeadCoordinates(this.head.Direction);
            this.head.Move(this.gameField);
            this.head.EatFood(cell);
        }
        #endregion

        #region Конструкторы

        public Snake(int x, int y, GameField gameField, int speed)
        {
            this.head = new SnakeHead(new FieldCoordinates(x, y), State.HeadDirection);
            this.Body = new List<SnakeMember>();
            this.gameField = gameField;
            this.Mind = new SnakeMind(this.Body, this.head, this.gameField);
            this.Speed = speed; 

            this.Mind.CreateSnake();

            this.Speed = speed;
        }

        #endregion
    }
}
