using System.Runtime.CompilerServices;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            State state = new State(15, 15, 1);

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
                            state.HeadDirection = "Left";
                            break;
                        case ConsoleKey.RightArrow:
                            state.HeadDirection = "Right";
                            break;
                        case ConsoleKey.UpArrow:
                            state.HeadDirection = "Up";
                            break;
                        case ConsoleKey.DownArrow:
                            state.HeadDirection = "Down";
                            break;
                        case ConsoleKey.Enter:
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