using System.Runtime.CompilerServices;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            State state = new State();
            Control ctrl = new Control(state);

            Task rendering = new Task(() => Render(state, ctrl));
            Task control = new Task(() =>
            {
                while (true)
                {
                    ctrl.KeyListener();
                }
            });


            ctrl.Move = ctrl.MoveDown;

            rendering.Start();
            control.Start();

            rendering.Wait();
            control.Wait();
        }

        static void Render(State state, Control control)
        {
            while (true)
            {
                Console.Clear();
                Console.SetCursorPosition(state.X, state.Y);
                Console.Write("Змеюга ползает");
                control.Move();
                Thread.Sleep(500);
            }

        }
    }
}