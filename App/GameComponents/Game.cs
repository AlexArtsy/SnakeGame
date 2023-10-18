using SnakeGame.App.Field;
using SnakeGame.App.GameComponents.OperationController;
using SnakeGame.App.GameComponents.ViewController;
using SnakeGame.App.Gamer;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.GameComponents
{
    public class Game
    {
        #region Поля
        #endregion

        #region Свойства
        public State State { get; set; }
        public GameField GameField { get; set; }
        public IViewer Rendering { get; set; }
        public IController GameControl { get; set; }
        public Snake Snake { get; set; }
        #endregion

        #region Методы
        public void Run()
        {
            this.Rendering.Clear(); //  Спорный момент, но допустим...

            var runSnake = new Task(RunSnake);
            var generateFood = new Task(() => this.GameField.GenerateFood(this.State));

            var controlling = new Task(this.GameControl.Run);

            runSnake.Start();
            generateFood.Start();
            controlling.Start();

            runSnake.Wait();
            generateFood.Wait();
            controlling.Wait();
        }

        private void RunSnake()
        {
            while (this.State.IsSnakeAlive)
            {
                this.Snake.RunSnake();
                this.Rendering.UpdateField(this.GameField);

                Thread.Sleep(this.State.GameTickTimeValue);
            }
        }

        #region GameControl
        //public void UpdateHeadDirection(string direction)
        //{
        //    switch (direction)
        //    {
        //        case "Left":
        //            State.HeadDirection = State.HeadDirection == "Right" ? "Right" : "Left";
        //            break;
        //        case "Right":
        //            State.HeadDirection = State.HeadDirection == "Left" ? "Left" : "Right";
        //            break;
        //        case "Up":
        //            State.HeadDirection = State.HeadDirection == "Down" ? "Down" : "Up";
        //            break;
        //        case "Down":
        //            State.HeadDirection = State.HeadDirection == "Up" ? "Up" : "Down";
        //            break;
        //        default:
        //            break;
        //    }
        //}
        public void IncreaseSpeed()
        {
            State.SnakeSpeed += 50;
        }
        public void IncreaseScore()
        {
            State.GameScore += 100;
        }
        public void DecreaseFoodValue()
        {
            State.FoodPiecesValue -= 1;
        }
        public void DecreaseScore()
        {
            State.GameScore -= 1;
        }
        public void KillSnake()
        {
            State.IsSnakeAlive = false;
        }
        #endregion
        #endregion

        #region Конструкторы
        public Game(State state, GameField field, IViewer rendering, IController control)
        {
            this.State = state;
            this.GameField = field;

            this.GameControl = control;
            this.Rendering = rendering;

            this.Snake = new Snake(5, 5, this.GameField, this.State);
        }
        #endregion
    }
}
