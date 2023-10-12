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

            var trainer = new Trainer(new int[] {100, 150, 3});

            var net = new Network(209,new int[] { 100, 150, 3 });

            trainer.Network = net;
            trainer.BackTrainer = new BackPropagationTrainer(net);

            var field = new GameField(3, 5, 20, 10);

            trainer.Train(field, 0.005);

            

            //var gamer = new Robot(field);
            //var gamer = new Human(field);
            var gamer = new AiGamer(net, field);

            var game = new Game(gamer, field);
            game.Run();



        }

        //public static List<DataSet> GetDataSet(GameField field)
        //{
        //    FieldEmptiness emptiness = new FieldEmptiness();
        //    SnakeBodyPart snake = new SnakeBodyPart(new FieldCoordinates(0,0));
        //    SnakeHead head = new SnakeHead(new FieldCoordinates(0, 0), "Up");
        //    FieldWall wall = new FieldWall();
        //    SnakeFood food = new SnakeFood();

        //    List<DataSet> dataSet = new List<DataSet>();


        //    List<IFieldCellValue> emptinessSet = new List<IFieldCellValue>();
        //    emptinessSet.Add(emptiness);
        //    emptinessSet.Add(emptiness);
        //    emptinessSet.Add(emptiness);

        //    emptinessSet.Add(emptiness);
        //    emptinessSet.Add(head);
        //    emptinessSet.Add(emptiness);

        //    emptinessSet.Add(emptiness);
        //    emptinessSet.Add(snake);
        //    emptinessSet.Add(emptiness);
        //    emptinessSet = AddFieldToDataSet(field, emptinessSet);
        //    double[] emptinessTarget = new double[] { 0, 1, 0 };


        //    List<IFieldCellValue> wallSet = new List<IFieldCellValue>();
        //    wallSet.Add(wall);
        //    wallSet.Add(wall);
        //    wallSet.Add(wall);

        //    wallSet.Add(emptiness);
        //    wallSet.Add(head);
        //    wallSet.Add(emptiness);

        //    wallSet.Add(emptiness);
        //    wallSet.Add(snake);
        //    wallSet.Add(emptiness);
        //    wallSet = AddFieldToDataSet(field, wallSet);
        //    double[] wallTarget = new double[] { 0, 0, 1 };


        //    List<IFieldCellValue> foodCenterSet = new List<IFieldCellValue>();
        //    //foodCenterSet = AddAreaToDataSet(foodCenterSet, new IFieldCellValue[] { emptiness, food, emptiness, })
        //    foodCenterSet.Add(emptiness);
        //    foodCenterSet.Add(food);
        //    foodCenterSet.Add(emptiness);

        //    foodCenterSet.Add(emptiness);
        //    foodCenterSet.Add(head);
        //    foodCenterSet.Add(emptiness);

        //    foodCenterSet.Add(emptiness);
        //    foodCenterSet.Add(snake);
        //    foodCenterSet.Add(emptiness);
        //    foodCenterSet = AddFieldToDataSet(field, foodCenterSet);
        //    double[] foodCenterTarget = new double[] { 0, 1, 0 };


        //    List<IFieldCellValue> foodLeftSet = new List<IFieldCellValue>();
        //    foodLeftSet.Add(emptiness);
        //    foodLeftSet.Add(emptiness);
        //    foodLeftSet.Add(emptiness);

        //    foodLeftSet.Add(food);
        //    foodLeftSet.Add(head);
        //    foodLeftSet.Add(emptiness);

        //    foodLeftSet.Add(emptiness);
        //    foodLeftSet.Add(snake);
        //    foodLeftSet.Add(emptiness);
        //    foodLeftSet = AddFieldToDataSet(field, foodLeftSet);
        //    double[] foodLeftTarget = new double[] { 1, 0, 0 };

        //    List<IFieldCellValue> foodLeftUpSet = new List<IFieldCellValue>();
        //    foodLeftUpSet.Add(food);
        //    foodLeftUpSet.Add(emptiness);
        //    foodLeftUpSet.Add(emptiness);

        //    foodLeftUpSet.Add(emptiness);
        //    foodLeftUpSet.Add(head);
        //    foodLeftUpSet.Add(emptiness);

        //    foodLeftUpSet.Add(emptiness);
        //    foodLeftUpSet.Add(snake);
        //    foodLeftUpSet.Add(emptiness);
        //    foodLeftUpSet = AddFieldToDataSet(field, foodLeftUpSet);
        //    double[] foodLeftUpTarget = new double[] { 1, 0, 0 };


        //    List<IFieldCellValue> foodRightSet = new List<IFieldCellValue>();
        //    foodRightSet.Add(emptiness);
        //    foodRightSet.Add(emptiness);
        //    foodRightSet.Add(emptiness);

        //    foodRightSet.Add(emptiness);
        //    foodRightSet.Add(head);
        //    foodRightSet.Add(food);

        //    foodRightSet.Add(emptiness);
        //    foodRightSet.Add(snake);
        //    foodRightSet.Add(emptiness);
        //    foodRightSet = AddFieldToDataSet(field, foodRightSet);
        //    double[] foodRightTarget = new double[] { 0, 0, 1 };

        //    List<IFieldCellValue> foodRightUpSet = new List<IFieldCellValue>();
        //    foodRightUpSet.Add(emptiness);
        //    foodRightUpSet.Add(emptiness);
        //    foodRightUpSet.Add(emptiness);

        //    foodRightUpSet.Add(emptiness);
        //    foodRightUpSet.Add(head);
        //    foodRightUpSet.Add(food);

        //    foodRightUpSet.Add(emptiness);
        //    foodRightUpSet.Add(snake);
        //    foodRightUpSet.Add(emptiness);
        //    foodRightUpSet = AddFieldToDataSet(field, foodRightUpSet);
        //    double[] foodRightUpTarget = new double[] { 0, 0, 1 };


        //    List<IFieldCellValue> wall1 = new List<IFieldCellValue>();
        //    wall1.Add(wall);
        //    wall1.Add(wall);
        //    wall1.Add(wall);

        //    wall1.Add(wall);
        //    wall1.Add(head);
        //    wall1.Add(emptiness);

        //    wall1.Add(wall);
        //    wall1.Add(snake);
        //    wall1.Add(emptiness);
        //    wall1 = AddFieldToDataSet(field, wall1);
        //    double[] wall1Target = new double[] { 0, 0, 1 };


        //    List<IFieldCellValue> wall2 = new List<IFieldCellValue>();
        //    wall2.Add(wall);
        //    wall2.Add(wall);
        //    wall2.Add(wall);

        //    wall2.Add(emptiness);
        //    wall2.Add(head);
        //    wall2.Add(wall);

        //    wall2.Add(emptiness);
        //    wall2.Add(snake);
        //    wall2.Add(wall);
        //    wall2 = AddFieldToDataSet(field, wall2);
        //    double[] wall2Target = new double[] { 1, 0, 0 };


        //    List<IFieldCellValue> wall3 = new List<IFieldCellValue>();  // вдоль стены (стена справа)
        //    wall3.Add(emptiness);
        //    wall3.Add(emptiness);
        //    wall3.Add(wall);

        //    wall3.Add(emptiness);
        //    wall3.Add(head);
        //    wall3.Add(wall);

        //    wall3.Add(emptiness);
        //    wall3.Add(snake);
        //    wall3.Add(wall);
        //    wall3 = AddFieldToDataSet(field, wall3);
        //    double[] wall3Target = new double[] { 1, 0, 0 };


        //    List<IFieldCellValue> wall4 = new List<IFieldCellValue>();  // вдоль стены (стена слева)
        //    wall4.Add(wall);
        //    wall4.Add(emptiness);
        //    wall4.Add(emptiness);

        //    wall4.Add(wall);
        //    wall4.Add(head);
        //    wall4.Add(emptiness);

        //    wall4.Add(wall);
        //    wall4.Add(snake);
        //    wall4.Add(emptiness);
        //    wall4 = AddFieldToDataSet(field, wall4);
        //    double[] wall4Target = new double[] { 0, 0, 1 };

        //    var values = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, wall, 
        //        emptiness, head, wall, 
        //        emptiness, snake, wall,
        //    };
        //    var wallEmptiness = new List<IFieldCellValue>();
        //    wallEmptiness = AddAreaToDataSet(wallEmptiness, values);
        //    wallEmptiness = AddFieldToDataSet(field, wallEmptiness);
        //    //   Доделать


        //    dataSet.Add(new DataSet(emptinessSet, emptinessTarget));
        //    dataSet.Add(new DataSet(wallSet, wallTarget));
        //    dataSet.Add(new DataSet(foodLeftSet, foodLeftTarget));
        //    dataSet.Add(new DataSet(foodLeftUpSet, foodLeftUpTarget));
        //    dataSet.Add(new DataSet(foodCenterSet, foodCenterTarget));
        //    dataSet.Add(new DataSet(foodRightSet, foodRightTarget));
        //    dataSet.Add(new DataSet(foodRightUpSet, foodRightUpTarget));
        //    dataSet.Add(new DataSet(wall1, wall1Target));   //  переименовать на райт и лефт  
        //    dataSet.Add(new DataSet(wall2, wall2Target));   //  переименовать на райт и лефт  
        //    dataSet.Add(new DataSet(wall3, wall3Target));   //  переименовать на райт и лефт  
        //    dataSet.Add(new DataSet(wall4, wall4Target));   //  переименовать на райт и лефт  

        //    return dataSet;
        //}

        //public static List<IFieldCellValue> AddFieldToDataSet(GameField field, List<IFieldCellValue> fieldValues)
        //{
        //    for (var y = 0; y < field.height; y += 1)
        //    {
        //        for (var x = 0; x < field.width; x += 1)
        //        {
        //            fieldValues.Add(field.Field[x, y].Value);
        //        }
        //    }
        //    return fieldValues;
        //}
        //public static List<IFieldCellValue> AddAreaToDataSet(List<IFieldCellValue> fieldValues, IFieldCellValue[] values)
        //{
        //    for (var i = 0; i < values.Length; i += 1)
        //    {
        //        fieldValues.Add(values[i]);
        //    }
        //    return fieldValues;
        //}
    }
}