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
            string networkname = "209-300-2";
            var network = Network.ReadNetworkFromFileOrCreate(networkname, 209, new int[] { 300, 2 });
            

            var gameState = new State();
            var rendering = new ConsoleRendering(gameState);
            var gameController = new ArrowKeyController(gameState);
            var field = new GameField(3, 5, 20, 10, gameState);
            var game = new Game(gameState, field, rendering, gameController);

            //var humanGamer = new Human(game);

            //RunHumanGame(game);
            RunTrainer(network);

            //var newGame = new Game(gameState, field, rendering, )
            RunAIGamer(game);
        }
        public static void RunHumanGame(Game game)
        {
            var gamer = new Human(game);
            gamer.Play();
        }

        public static void RunTrainer(Network network)
        {
            var state = new State();
            var gameField = new GameField(0, 0, 20, 10, state);
            var backTrainer = new BackPropagationTrainer(network, 0.05);
            var networkViewer = new NetworkViewController(network, state, new TrainingInfoRendering(state, network, backTrainer));
            var networkControl = new NetworkController(network, state, gameField, new Snake(0, 0, gameField, state), networkViewer);
            var trainer = new Trainer(networkControl, networkViewer)
            {
                BackTrainer = backTrainer
            };
        
            trainer.Run(network, gameField, 0.01);
        }

        public static void RunAIGamer(Game game)
        {
            var gamer = new AiGamer(game);
            gamer.Play();
        }
    }
}