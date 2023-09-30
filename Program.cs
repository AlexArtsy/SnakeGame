using System.Runtime.CompilerServices;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //var gamer = new Human();
            var gamer = new Robot();
            var game = new SnakeGame(gamer);
            game.Run();

        }
    }
}