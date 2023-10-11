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
            //var field = new GameField(5, 5, 20, 20);

            ////var gamer = new Robot(field);
            ////var gamer = new Human(field);
            //var gamer = new AiGamer(field);

            //var game = new Game(gamer, field);
            //game.Run();

            var trainer = new Trainer(new int[] {30, 3});

            //trainer.InitInputList();

            var net = new Network(9,new int[] { 40, 3 });

            trainer.Network = net;
            trainer.BackTrainer = new BackPropagationTrainer(net);



            trainer.Train(GetDataSet(), 0.01);

            var field = new GameField(5, 5, 20, 20);

            //var gamer = new Robot(field);
            //var gamer = new Human(field);
            var gamer = new AiGamer(net, field);

            var game = new Game(gamer, field);
            game.Run();



        }

        public static List<DataSet> GetDataSet()
        {
            FieldEmptiness emptiness = new FieldEmptiness();
            SnakeMember snake = new SnakeMember();
            FieldWall wall = new FieldWall();
            SnakeFood food = new SnakeFood();

            List<DataSet> dataSet = new List<DataSet>();


            List<IFieldCellValue> emptinessSet = new List<IFieldCellValue>();
            emptinessSet.Add(emptiness);
            emptinessSet.Add(emptiness);
            emptinessSet.Add(emptiness);

            emptinessSet.Add(emptiness);
            emptinessSet.Add(snake);
            emptinessSet.Add(emptiness);

            emptinessSet.Add(emptiness);
            emptinessSet.Add(snake);
            emptinessSet.Add(emptiness);
            double[] emptinessTarget = new double[] { 0, 1, 0 };


            List<IFieldCellValue> wallSet = new List<IFieldCellValue>();
            wallSet.Add(wall);
            wallSet.Add(wall);
            wallSet.Add(wall);

            wallSet.Add(emptiness);
            wallSet.Add(snake);
            wallSet.Add(emptiness);

            wallSet.Add(emptiness);
            wallSet.Add(snake);
            wallSet.Add(emptiness);
            double[] wallTarget = new double[] { 0, 0, 1 };


            List<IFieldCellValue> foodCenterSet = new List<IFieldCellValue>();
            foodCenterSet.Add(emptiness);
            foodCenterSet.Add(food);
            foodCenterSet.Add(emptiness);

            foodCenterSet.Add(emptiness);
            foodCenterSet.Add(snake);
            foodCenterSet.Add(emptiness);

            foodCenterSet.Add(emptiness);
            foodCenterSet.Add(snake);
            foodCenterSet.Add(emptiness);
            double[] foodCenterTarget = new double[] { 0, 1, 0 };


            List<IFieldCellValue> foodLeftSet = new List<IFieldCellValue>();
            foodLeftSet.Add(food);
            foodLeftSet.Add(emptiness);
            foodLeftSet.Add(emptiness);

            foodLeftSet.Add(emptiness);
            foodLeftSet.Add(snake);
            foodLeftSet.Add(emptiness);

            foodLeftSet.Add(emptiness);
            foodLeftSet.Add(snake);
            foodLeftSet.Add(emptiness);
            double[] foodLeftTarget = new double[] { 1, 0, 0 };


            List<IFieldCellValue> foodRightSet = new List<IFieldCellValue>();
            foodRightSet.Add(emptiness);
            foodRightSet.Add(emptiness);
            foodRightSet.Add(food);

            foodRightSet.Add(emptiness);
            foodRightSet.Add(snake);
            foodRightSet.Add(emptiness);

            foodRightSet.Add(emptiness);
            foodRightSet.Add(snake);
            foodRightSet.Add(emptiness);
            double[] foodRightTarget = new double[] { 0, 0, 1 };


            List<IFieldCellValue> wall1 = new List<IFieldCellValue>();
            wall1.Add(wall);
            wall1.Add(wall);
            wall1.Add(wall);

            wall1.Add(wall);
            wall1.Add(snake);
            wall1.Add(emptiness);

            wall1.Add(wall);
            wall1.Add(snake);
            wall1.Add(emptiness);
            double[] wall1Target = new double[] { 0, 0, 1 };


            List<IFieldCellValue> wall2 = new List<IFieldCellValue>();
            wall2.Add(wall);
            wall2.Add(wall);
            wall2.Add(wall);

            wall2.Add(emptiness);
            wall2.Add(snake);
            wall2.Add(wall);

            wall2.Add(emptiness);
            wall2.Add(snake);
            wall2.Add(wall);
            double[] wall2Target = new double[] { 1, 0, 0 };


            List<IFieldCellValue> wall3 = new List<IFieldCellValue>();  // вдоль стены (стена справа)
            wall3.Add(emptiness);
            wall3.Add(emptiness);
            wall3.Add(wall);

            wall3.Add(emptiness);
            wall3.Add(snake);
            wall3.Add(wall);

            wall3.Add(emptiness);
            wall3.Add(snake);
            wall3.Add(wall);
            double[] wall3Target = new double[] { 1, 0, 0 };


            List<IFieldCellValue> wall4 = new List<IFieldCellValue>();  // вдоль стены (стена слева)
            wall4.Add(wall);
            wall4.Add(emptiness);
            wall4.Add(emptiness);

            wall4.Add(wall);
            wall4.Add(snake);
            wall4.Add(emptiness);

            wall4.Add(wall);
            wall4.Add(snake);
            wall4.Add(emptiness);
            double[] wall4Target = new double[] { 0, 0, 1 };



            dataSet.Add(new DataSet(emptinessSet, emptinessTarget));
            dataSet.Add(new DataSet(wallSet, wallTarget));
            dataSet.Add(new DataSet(foodLeftSet, foodLeftTarget));
            dataSet.Add(new DataSet(foodCenterSet, foodCenterTarget));
            dataSet.Add(new DataSet(foodRightSet, foodRightTarget));
            dataSet.Add(new DataSet(wall1, wall1Target));   //  переименовать на райт и лефт  
            dataSet.Add(new DataSet(wall2, wall2Target));   //  переименовать на райт и лефт  
            dataSet.Add(new DataSet(wall3, wall1Target));   //  переименовать на райт и лефт  
            dataSet.Add(new DataSet(wall4, wall2Target));   //  переименовать на райт и лефт  

            return dataSet;
        }
    }
}