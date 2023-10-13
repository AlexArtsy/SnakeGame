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
            //var trainer = new Trainer();    //  Добавить тип обучения ITrainer
            //var network = Network.ReadNetworkFromFileOrCreate("test1", 209,new int[] { 100, 150, 3 });
            //trainer.BackTrainer = new BackPropagationTrainer(network, 0.5);
            var field = new GameField(3, 5, 20, 10);
            //trainer.Run(network, field, 0.1);

            //var gamer = new Robot(field);
            var gamer = new Human(field);
            //var gamer = new AiGamer(network, field);

            var game = new Game(gamer, field);
            game.Run();
        }
    }
}