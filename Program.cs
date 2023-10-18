using System.Runtime.CompilerServices;
using SnakeGame.App;
using SnakeGame.App.Field;
using SnakeGame.App.GameComponents;
using SnakeGame.App.GameComponents.OperationController;
using SnakeGame.App.GameComponents.ViewController;
using SnakeGame.App.Gamer;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.Neural.Training;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame
{
    class Program
    {
        static void Main(string[] args)
        {
            string networkname = "209-400-300-7";
            var network = Network.ReadNetworkFromFileOrCreate(networkname, 209, new int[] { 400, 300, 7 });
            

            var gameState = new State();
            var rendering = new ConsoleRendering(gameState);
            var gameController = new ArrowKeyController(gameState);
            var field = new GameField(3, 5, 20, 10);
            var game = new Game(gameState, field, rendering, gameController);

            //var humanGamer = new Human(game);

            RunHumanGame(game);
            //RunTrainer(network);
            //RunAIGamer(field, network);
        }
        public static void RunHumanGame(Game game)
        {
            var gamer = new Human(game);
            gamer.Play();
        }

        public static void RunTrainer(Game game)
        {
            //var trainer = new Trainer();
            //trainer.BackTrainer = new BackPropagationTrainer(network, 0.05);
            //trainer.Run(network, field, 0.01);

            //var gamer = new AiGamer(network, field);
            //gamer.Play();

        }

        public static void RunAIGamer(Game game)
        {
            //var gamer = new AiGamer(game, rendering, control);
            //gamer.Play();
        }
    }
}