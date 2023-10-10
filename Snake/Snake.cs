
using Microsoft.VisualBasic;

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
            while (State.IsSnakeAlive)
            {
                //var cell = this.Mind.ExploreNextCell();    //  голова поглощает ячейки в любом случае каждый раз, просто реакция на содержимое - разная
                //var cellValue = cell.Value;

                this.Mind.ReadDirection();
                this.Mind.SetSpeed();
                this.Mind.SetNextHeadCoordinates(this.head.Direction);
                this.Mind.CalculateBodyMovingCoordinates();

                var cell = this.Mind.ExploreNextCell();    //  голова поглощает ячейки в любом случае каждый раз, просто реакция на содержимое - разная
                //var cellValue = this.Mind.ExploreNextCell().Value;
                var food = cell.Value;
                
                Move();
                EatFood(food, cell);

                SnakeMoved?.Invoke();

                Thread.Sleep(1000 - State.SnakeSpeed);

                
            }
        }
        public void Move()
        {
            this.Body.ForEach(p =>
            {
                p.Move(this.gameField);
            });
        }

        private void EatFood(IFieldCellValue food, FieldCell cell)
        {
            this.head.Eat(food, cell, this);
        }

        public void RaiseSnake(FieldCell cell)
        {
            this.Body.Insert(1, new SnakeBodyPart(this.head.Position));
            this.Mind.SetNextHeadCoordinates(this.head.Direction);
            this.head.Move(this.gameField);
            //this.head.EatFood(cell);

            SnakeRised?.Invoke();
            Raised?.Invoke(cell, ConsoleColor.Green);
        }

        public void Die(FieldCell cell)
        {
            Crashed?.Invoke(cell, ConsoleColor.Red);
            SnakeDies?.Invoke();
        }
        #endregion

        #region Делегаты
        public delegate void SnakeVisualHandler(FieldCell cell, ConsoleColor color);
        public delegate void SnakeHandler();
        #endregion

        #region События
        public event SnakeVisualHandler Raised;
        public event SnakeVisualHandler Crashed;
        public event SnakeHandler SnakeRised;
        public event SnakeHandler SnakeMoved;
        public event SnakeHandler SnakeDies;
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

            Raised += RenderProcessor.Blink;
            Crashed += RenderProcessor.Blink;
            //SnakeRised += RenderProcessor.ShowScore;
            SnakeRised += RenderProcessor.ShowSpeed;
            SnakeRised += Control.DecreaseFoodValue;
            SnakeRised += Control.IncreaseSpeed;
            SnakeMoved += Control.DecreaseScore;
            SnakeMoved += RenderProcessor.ShowScore;
        }

        #endregion
    }
}
