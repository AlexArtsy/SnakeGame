namespace SnakeGame.App.GameComponents.OperationController
{
    public class NumPadKeyController : IController
    {
        public State State { get; set; }
        public void Run()
        {
            while (this.State.IsSnakeAlive)
            {
                this.State.HeadDirection = DirectionGenerator();
            }
        }
        public string DirectionGenerator()
        {
            var direction = "";
            var pressedKey = Console.ReadKey(true);

            switch (pressedKey.Key)
            {
                case ConsoleKey.Tab:
                    break;
                case ConsoleKey.NumPad4:
                    direction = "Left";
                    break;
                case ConsoleKey.NumPad6:
                    direction = "Right";
                    break;
                case ConsoleKey.NumPad8:
                    direction = "Up";
                    break;
                case ConsoleKey.NumPad2:
                    direction = "Down";
                    break;
                default:
                    break;
            }
            return direction;
        }
        public NumPadKeyController(State state)
        {
            this.State = state;
        }
    }
}
