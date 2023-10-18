using SnakeGame.App.Field;
using SnakeGame.App.GameComponents;

namespace SnakeGame.App.SnakeComponents
{
    public class Snake
    {
        #region Поля
        private readonly GameField gameField;
        private bool isSnakeRised = false;
        public SnakeHead head;
        #endregion

        #region Свойства
        public SnakeMind Mind { get; set; }
        public State State { get; set; }
        public List<SnakeMember> Body { get; set; }

        #endregion

        #region Методы

        public void RunSnake()
        {
            ThinkBeforeMoving();

            var cell = ExploreArea();
            var food = cell.Value;

            Move();
            EatFood(food, cell);
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
            this.State.GameScore = this.State.GameScore == 0 ? 0: this.State.GameScore - 1;

            if (!isSnakeRised)  //  Для более красивой отрисовки при поедании еды
            {
                Body.ForEach(p =>
                {
                    p.Figure = p.Figure == "O" ? "o" : "O";
                    p.Move(gameField);
                });
                this.isSnakeRised = false;
                this.head.Figure = this.Body[1].Figure == "O" ? "s" : "S";
                return;
            }

            this.head.Move(gameField);
            this.head.Figure = this.Body[1].Figure == "O" ?  "s" : "S";
            this.isSnakeRised = false;
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
            var newBodyPart = new SnakeBodyPart(head.Position);
            newBodyPart.Figure = this.Body[1].Figure == "O" ? "o" : "O";

            Body.Insert(1, newBodyPart);

            cell.Value = newBodyPart;
            this.isSnakeRised = true;

            this.State.GameTickTimeValue -= 50;
            this.State.SnakeSpeed += 50;
            this.State.GameScore += 100;
        }

        public void Die(FieldCell cell)
        {
            this.State.IsSnakeAlive = false;
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
        public Snake(int x, int y, GameField gameField, State state)
        {
            this.State = state;
                this.head = new SnakeHead(new FieldCoordinates(x, y), State.HeadDirection);
            this.Body = new List<SnakeMember>();
            this.gameField = gameField;
            this.Mind = new SnakeMind(Body, head, this.gameField, this.State);

            GiveBirthToSnake();
        }

        public Snake(SnakeHead head, SnakeBodyPart part)
        {
            this.head = head;
            this.Body = new List<SnakeMember>();
            this.Body.Add(head);
            this.Body.Add(part);
        }

        #endregion
    }
}
