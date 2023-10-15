using System.Runtime.CompilerServices;
using SnakeGame.App;
using SnakeGame.App.Field;
using SnakeGame.App.Game;
using SnakeGame.App.Gamer;
using SnakeGame.App.Neural;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            //string networkname = "409-300-9";
            string networkname = "209-400-300-7";
            var network = Network.ReadNetworkFromFileOrCreate(networkname, 209, new int[] { 400, 300, 7 });
            var field = new GameField(3, 5, 20, 10);
            //RunHumanGame(field);
            RunTrainer(networkname, field, network);
            //RunAIGamer(networkname, field, network);
        }
        public static void RunHumanGame(GameField field)
        {
            var gamer = new Human(field);
            var game = new Game(gamer, field);
            game.Run();
        }

        public static void RunTrainer(string networkname, GameField field, Network network)
        {
            var trainer = new Trainer();
            trainer.BackTrainer = new BackPropagationTrainer(network, 0.05);
            trainer.Run(network, field, 0.01);

            var gamer = new AiGamer(network, field);

            var game = new Game(gamer, field);
            game.Run();
        }

        public static void RunAIGamer(string networkname, GameField field, Network network)
        {
            
            var gamer = new AiGamer(network, field);

            var game = new Game(gamer, field);
            game.Run();
        }
    }
}