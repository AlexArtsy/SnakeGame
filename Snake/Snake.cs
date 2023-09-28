
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
            Task snakeTask = new Task(() =>
            {
                while (true)
                {
                    Move();
                    var next = this.Mind.ExploreNextCell();
                    if (next.Value.ToString() == "SnakeGame.SnakeFood")
                    {
                        RaiseSnake((SnakeFood)next.Value);
                    }

                    Thread.Sleep(this.head.Speed * 10);
                }
            });

            snakeTask.Start();

            snakeTask.Wait();

        }
        public void Move()
        {
            this.Mind.SetNextHeadCoordinates(this.head.Direction);
            this.Mind.CalculateBodyMovingCoordinates();

            this.Body.ForEach(p =>
            {
                p.Move(this.gameField);
            });

        }

        public void RaiseSnake(SnakeFood food)
        {
            this.Body.Insert(1, new SnakeBodyPart(this.head.Position));
            this.Mind.SetNextHeadCoordinates(this.head.Direction);
            this.head.Move(this.gameField);
            this.head.EatFood(food);
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
