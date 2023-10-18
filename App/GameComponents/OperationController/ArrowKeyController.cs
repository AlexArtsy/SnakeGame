using System.Xml;
using SnakeGame.App.Field;

namespace SnakeGame.App.GameComponents.OperationController
{
    public class ArrowKeyController : IController
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
                case ConsoleKey.LeftArrow:
                    direction = "Left";
                    break;
                case ConsoleKey.RightArrow:
                    direction = "Right";
                    break;
                case ConsoleKey.UpArrow:
                    direction = "Up";
                    break;
                case ConsoleKey.DownArrow:
                    direction = "Down";
                    break;
                default:
                    break;
            }
            return direction;
        }

        public ArrowKeyController(State state)
        {
            this.State = state;
        }
    }
}
