using System.Runtime.CompilerServices;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            State state = new State(15, 15, 1);
            state.fieldWidth = 10;
            state.fieldHeight = 10;

            var game = new SnakeGame(state);

            Task task1 = new Task(() => game.Run());
            Task task2 = new Task(() =>
            {
                while (true)
                {
                    var pressedKey = Console.ReadKey(true);
                    switch (pressedKey.Key)
                    {
                        case ConsoleKey.Tab:
                            break;
                        case ConsoleKey.LeftArrow:
                            State.HeadDirection = "Left";
                            break;
                        case ConsoleKey.RightArrow:
                            State.HeadDirection = "Right";
                            break;
                        case ConsoleKey.UpArrow:
                            State.HeadDirection = "Up";
                            break;
                        case ConsoleKey.DownArrow:
                            State.HeadDirection = "Down";
                            break;
                        case ConsoleKey.PageUp:
                            State.SnakeSpeed = State.SnakeSpeed == 0 ? 0 : State.SnakeSpeed - 20;
                            break;
                        case ConsoleKey.PageDown:
                            State.SnakeSpeed += 20;
                            break;
                        case ConsoleKey.Backspace:
                            break;
                        default:
                            break;
                    }
                }
            });

            task1.Start();
            task2.Start();

            task1.Wait();
            task2.Wait();

        }
    }
}