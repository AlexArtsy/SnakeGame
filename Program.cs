using System.Runtime.CompilerServices;
using NeuroCompote;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var field = new GameField(5, 5, 20, 20);

            //var gamer = new Robot(field);
            //var gamer = new Human(field);
            var gamer = new AiGamer(field);

            var game = new SnakeGame(gamer, field);
            game.Run();

        }
    }
}