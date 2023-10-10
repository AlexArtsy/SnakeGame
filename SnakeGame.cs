namespace SnakeGame
{
    internal class SnakeGame
    {
        #region Поля
        private readonly GameField field;
        private readonly Control gameControl;
        private RenderProcessor rendering;
        private readonly Snake.Snake snake;
        #endregion

        #region Свойства
        public IGamer Gamer { get; set; }
        #endregion

        #region Методы
        public void Run()
        {
            var task1 = new Task(() => this.Gamer.Play());
            var task2 = new Task(() => this.snake.RunSnake());
            var task3 = new Task(() => this.field.GenerateFood());

            task1.Start();
            task2.Start();
            task3.Start();

            task1.Wait();
            task2.Wait();
            task3.Wait();
        }

        private void IncreaseSpeed()
        {
            State.SnakeSpeed += 50;
        }
        #endregion

        #region Конструкторы
        public SnakeGame(IGamer gamer, GameField field)
        {
            this.Gamer = gamer;

            this.field = field;
            //this.rendering = new RenderProcessor();
            //RenderProcessor.SubscribeFieldCellChangingEvent(this.field);
            this.gameControl = new Control();

            this.snake = new Snake.Snake(5, 3, this.field, 100);
            this.snake.SnakeRised += IncreaseSpeed;

            this.Gamer.Snake = this.snake;

            this.snake.SnakeDies += Control.KillSnake;
        }
        #endregion
    }
}
