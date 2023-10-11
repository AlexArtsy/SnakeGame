using SnakeGame.App.Field;
using SnakeGame.App.Game;

namespace SnakeGame.App.SnakeComponents
{
    public class Snake
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

                ThinkBeforeMoving();

                var cell = ExploreArea();
                var food = cell.Value;

                Move();
                EatFood(food, cell);

                SnakeMoved?.Invoke();

                Thread.Sleep(1000 - State.SnakeSpeed);
                //var pressedKey = Console.ReadKey(true);


            }
        }

        private FieldCell ExploreArea()
        {
            return Mind.ExploreNextCell();
        }

        private void ThinkBeforeMoving()
        {
            Mind.ReadDirection();
            Mind.SetSpeed();
            Mind.SetNextHeadCoordinates(head.Direction);
            Mind.CalculateBodyMovingCoordinates();
        }

        private void Move()
        {
            Body.ForEach(p =>
            {
                p.Move(gameField);
            });
        }

        private void GiveBirthToSnake()
        {
            this.Mind.CreateSnake();
        }

        private void EatFood(IFieldCellValue food, FieldCell cell)
        {
            head.Eat(food, cell, this);
        }

        public void RaiseSnake(FieldCell cell)
        {
            SnakeRised?.Invoke();
            Raised?.Invoke(cell, ConsoleColor.Green);

            var newBodyPart = new SnakeBodyPart(head.Position);
            //cell.Value = newBodyPart;

            Body.Insert(1, newBodyPart);
            
            Mind.SetNextHeadCoordinates(head.Direction);
            
            head.Move(gameField);

            cell.Value = newBodyPart;
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
            this.Mind = new SnakeMind(Body, head, this.gameField);
            this.Speed = speed;

            State.SnakeSpeed = this.Speed; // Переделать!!!

            GiveBirthToSnake();
        }

        #endregion
    }
}
