using System.Runtime.CompilerServices;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            State state = new State();

            Task rendering = new Task(() => Render(state));
            Task control = new Task(() => Control(state));

            state.Move = state.MoveDown;

            rendering.Start();
            control.Start();

            rendering.Wait();
            control.Wait();
        }

        static void Render(State state)
        {
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(state.X, state.Y);
                Console.Write("Змеюга ползает");
                state.Move();
                Thread.Sleep(500);
            }

        }

        static void Control(State state)
        {
            while (true)
            {
                var pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Tab:
                        break;
                    case ConsoleKey.LeftArrow:
                        state.Move = state.MoveLeft;
                        break;
                    case ConsoleKey.RightArrow:
                        state.Move = state.MoveRight;
                        break;
                    case ConsoleKey.UpArrow:
                        state.Move = state.MoveUp;
                        break;
                    case ConsoleKey.DownArrow:
                        state.Move = state.MoveDown;
                        break;
                    case ConsoleKey.Enter:
                        break;
                    case ConsoleKey.Backspace:
                        break;
                    default:
                        break;
                }
            }
        }
    }
}