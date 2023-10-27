using SnakeGame.App.Field;
using SnakeGame.App.SnakeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using SnakeGame.App.GameComponents;
using SnakeGame.App.GameComponents.OperationController;
using SnakeGame.App.GameComponents.ViewController;
using SnakeGame.App.Neural.NetworkComponents;

namespace SnakeGame.App.Neural.Training
{
    public class Trainer
    {
        #region Поля
        #endregion

        #region Свойства
        public List<DataSet> DataSet { get; set; } = new List<DataSet>();
        public NetworkController NetworkControl { get; set; }
        public NetworkViewController FieldViewer { get; set; }
        public BackPropagationTrainer BackTrainer { get; set; }
        #endregion

        #region Методы
        public void UpdateDataFile(Network network)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{network.Name}.txt";
            var data = JsonSerializer.Serialize(network);
            File.WriteAllText(path, data);
        }

        private void Train(Network network, DataSet data, GameField field)
        {
            var inputsVector = data.InputData;
            network.Calculate(inputsVector);
            network.TotalError = BackTrainer.Train(data.Target);

            network.SumForAvgError += network.TotalError;

            network.ValueOfLearningCycles += 1;

            network.AvgError = network.SumForAvgError / network.ValueOfLearningCycles;

            network.HistoryOfAvgError.Add(network.AvgError);


            this.FieldViewer.UpdateField(field); // Фигня какая-то получается

            //Console.ReadKey(true);

        }

        public void Run(Network network, GameField field, double fidelity)
        {
            network.isTrainingMode = true;

            SetUpManualDataSet(field);

            Task keyControl = new Task(() => ProcessKeyControl(network));
            //Task render = new Task(() =>
            //{
            //    while (network.isTrainingMode)
            //    {
            //        this.FieldViewer.UpdateField(field);
            //    }
            //});
            
            //render.Start();
            keyControl.Start();


            while (network.AvgError > fidelity & network.isTrainingMode)
            {
                DataSet.ForEach(manualData =>
                {
                    Train(network, manualData, field);

                    var randomData = CreateRandomTemplate(new GameField(field.X, field.Y, field.width, field.height, new State()), network);
                    Train(network, randomData, field);

     
                });
            }

            network.isTrainingMode = false;
            UpdateDataFile(network);

            keyControl.Wait();
        }

        #region DataSet Management
        public void SetUpManualDataSet(GameField field)
        {
            FieldEmptiness emptiness = new FieldEmptiness();
            SnakeBodyPart snake = new SnakeBodyPart(new FieldCoordinates(0, 0));
            SnakeHead head = new SnakeHead(new FieldCoordinates(0, 0), "Up");
            FieldWall wall = new FieldWall();
            SnakeFood food = new SnakeFood();

            var referenceLeft = new double[] { 1, 0 };
            var referenceCenter = new double[] { 0, 0 };
            var referenceRight = new double[] { 0, 1 };


            #region Движение в пустоте
            var template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, snake,
            };
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, emptiness,
                snake, snake, emptiness,
            };
            AddDataToDataSet(template, referenceCenter, field);
            #endregion

            #region Движение к еде

            template = new IFieldCellValue[]
            {
                emptiness, food, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, food,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRight, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, food,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRight, field);

            template = new IFieldCellValue[]
            {
                food, emptiness, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceLeft, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                food, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceLeft, field);

            #endregion

            #region Движение у(вдоль) стены и змеи
            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRight, field);

            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            //AddDataToDataSet(template, referenceLeft, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, wall,
                emptiness, head, wall,
                emptiness, snake, wall,
            };
            //AddDataToDataSet(template, referenceLeft, field);
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                wall, emptiness, emptiness,
                wall, head, emptiness,
                wall, snake, emptiness,
            };
            //AddDataToDataSet(template, referenceRight, field);
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                wall, head, emptiness,
                wall, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRight, field);

            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                emptiness, head, wall,
                emptiness, snake, wall,
            };
            AddDataToDataSet(template, referenceLeft, field);

            template = new IFieldCellValue[]
            {
                wall, emptiness, snake,
                wall, head, snake,
                wall, snake, snake,
            };
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                snake, emptiness, wall,
                snake, head, wall,
                snake, snake, wall,
            };
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                wall, emptiness, emptiness,
                wall, head, snake,
                wall, snake, snake,
            };
            AddDataToDataSet(template, referenceCenter, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, wall,
                snake, head, wall,
                snake, snake, wall,
            };
            AddDataToDataSet(template, referenceCenter, field);
            #endregion
        }
        //public void SetUpManualDataSet(GameField field)
        //{
        //    FieldEmptiness emptiness = new FieldEmptiness();
        //    SnakeBodyPart snake = new SnakeBodyPart(new FieldCoordinates(0, 0));
        //    SnakeHead head = new SnakeHead(new FieldCoordinates(0, 0), "Up");
        //    FieldWall wall = new FieldWall();
        //    SnakeFood food = new SnakeFood();

        //    var referenceLeftFree = new double[] { 0, 1, 0, 0, 0, 0, 0 };
        //    var referenceLeftFreeExsactement = new double[] { 1, 1, 0, 0, 0, 0, 0 };
        //    var referenceCenterFree = new double[] { 0, 0, 0, 1, 0, 0, 0 };
        //    var referenceCenterExsactement = new double[] { 0, 0, 1, 1, 1, 0, 0 };
        //    var referenceRightFree = new double[] { 0, 0, 0, 0, 0, 1, 0 };
        //    var referenceRightExactement = new double[] { 0, 0, 0, 0, 0, 1, 1 };


        //    #region Движение в пустоте
        //    var template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, emptiness,
        //        emptiness, head, emptiness,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceCenterFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, emptiness,
        //        emptiness, head, emptiness,
        //        emptiness, snake, snake,
        //    };
        //    AddDataToDataSet(template, referenceCenterFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, emptiness,
        //        emptiness, head, emptiness,
        //        snake, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceCenterFree, field);
        //    #endregion

        //    #region Движение к еде

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, food, emptiness,
        //        emptiness, head, emptiness,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceCenterExsactement, field);

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, food,
        //        emptiness, head, emptiness,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceRightFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, emptiness,
        //        emptiness, head, food,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceRightExactement, field);

        //    template = new IFieldCellValue[]
        //    {
        //        food, emptiness, emptiness,
        //        emptiness, head, emptiness,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceLeftFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, emptiness,
        //        food, head, emptiness,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceLeftFreeExsactement, field);

        //    #endregion

        //    #region Движение у(вдоль) стены и змеи
        //    template = new IFieldCellValue[]
        //    {
        //        wall, wall, wall,
        //        emptiness, head, emptiness,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceRightFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        wall, wall, wall,
        //        emptiness, head, emptiness,
        //        emptiness, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceLeftFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, wall,
        //        emptiness, head, wall,
        //        emptiness, snake, wall,
        //    };
        //    AddDataToDataSet(template, referenceLeftFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        wall, emptiness, emptiness,
        //        wall, head, emptiness,
        //        wall, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceRightFree, field);

        //    template = new IFieldCellValue[]
        //    {
        //        wall, wall, wall,
        //        wall, head, emptiness,
        //        wall, snake, emptiness,
        //    };
        //    AddDataToDataSet(template, referenceRightExactement, field);

        //    template = new IFieldCellValue[]
        //    {
        //        wall, wall, wall,
        //        emptiness, head, wall,
        //        emptiness, snake, wall,
        //    };
        //    AddDataToDataSet(template, referenceLeftFreeExsactement, field);

        //    template = new IFieldCellValue[]
        //    {
        //        wall, emptiness, snake,
        //        wall, head, snake,
        //        wall, snake, snake,
        //    };
        //    AddDataToDataSet(template, referenceCenterExsactement, field);

        //    template = new IFieldCellValue[]
        //    {
        //        snake, emptiness, wall,
        //        snake, head, wall,
        //        snake, snake, wall,
        //    };
        //    AddDataToDataSet(template, referenceCenterExsactement, field);

        //    template = new IFieldCellValue[]
        //    {
        //        wall, emptiness, emptiness,
        //        wall, head, snake,
        //        wall, snake, snake,
        //    };
        //    AddDataToDataSet(template, referenceCenterExsactement, field);

        //    template = new IFieldCellValue[]
        //    {
        //        emptiness, emptiness, wall,
        //        snake, head, wall,
        //        snake, snake, wall,
        //    };
        //    AddDataToDataSet(template, referenceCenterExsactement, field);
        //    #endregion
        //}

        public DataSet CreateRandomTemplate(GameField field, Network network)
        {
            var halfWidth = field.width / 2;
            var halfHeight = field.height / 2;

            FieldEmptiness emptiness = new FieldEmptiness();
            SnakeFood food = new SnakeFood();




            var referenceLeft = new double[] { 1, 0 };
            //var referenceCenter = new double[] { 0, 1, 0 };
            var referenceRight = new double[] { 0, 1};
            var reference = new double[] { };

            var randomSnake = GetRandomSnake(field);

            field.Field[randomSnake.head.Position.X, randomSnake.head.Position.Y].Value = randomSnake.head;
            field.Field[randomSnake.Body[1].Position.X, randomSnake.Body[1].Position.Y].Value = randomSnake.Body[1];

            

            var randomXFoodPosition = 0;
            var randomYFoodPosition = 0;

            var xFoodPosition = "";
            var yFoodPosition = "";


            if (randomSnake.head.Position.X < halfWidth)
            {
                randomXFoodPosition = RandomGen.GetRandomX(halfWidth) + halfWidth - 1;
                xFoodPosition = "Right";
            }
            else
            {
                randomXFoodPosition = RandomGen.GetRandomX(halfWidth) + 1;
                xFoodPosition = "Left";
            }

            if (randomSnake.head.Position.Y < halfHeight)
            {
                randomYFoodPosition = RandomGen.GetRandomX(halfHeight) + halfHeight - 1;
                yFoodPosition = "Down";
            }
            else
            {
                randomYFoodPosition = RandomGen.GetRandomX(halfHeight) + 1;
                yFoodPosition = "Up";
            }

            field.Field[randomXFoodPosition, randomYFoodPosition].Value = food;

            switch (randomSnake.head.Direction)
            {
                case "Up":
                    switch (xFoodPosition)
                    {
                        case "Right":
                            reference = referenceRight;
                            break;
                        case "Left":
                            reference = referenceLeft;
                            break;
                    }
                    break;
                case "Down":
                    switch (xFoodPosition)
                    {
                        case "Right":
                            reference = referenceLeft;
                            break;
                        case "Left":
                            reference = referenceRight;
                            break;
                    }
                    break;
                case "Right":
                    switch (yFoodPosition)
                    {
                        case "Up":
                            reference = referenceLeft;
                            break;
                        case "Down":
                            reference = referenceRight;
                            break;
                    }
                    break;
                case "Left":
                    switch (yFoodPosition)
                    {
                        case "Up":
                            reference = referenceRight;
                            break;
                        case "Down":
                            reference = referenceLeft;
                            break;
                    }
                    break;
            }

            var template = new List<IFieldCellValue>();

            var nearArea = this.FieldViewer.ScanArea(field.Field, randomSnake.head, randomSnake.head.Direction);

            AddFieldToTemplate(template, nearArea, 3, 3);
            AddFieldToTemplate(template, field.Field, field.width, field.height);

            return new DataSet(this.FieldViewer.GetNetworkInputsVector(this.FieldViewer.Network, template), reference);
        }

        public void AddDataToDataSet(IFieldCellValue[] template, double[] reference, GameField field)
        {
            var dataTemplate = new List<IFieldCellValue>();
            dataTemplate = AddNearAreaToTemplate(dataTemplate, template);
            AddFieldToTemplate(dataTemplate, field.Field, field.width, field.height);
            

            DataSet.Add(new DataSet(this.FieldViewer.GetNetworkInputsVector(this.FieldViewer.Network, dataTemplate), reference));
        }

        private void AddFieldToTemplate(List<IFieldCellValue> template, FieldCell[,] field, int width, int height)
        {
            for (var y = 0; y < height; y += 1)
            {
                for (var x = 0; x < width; x += 1)
                {
                    for (var s = 0; s < 30; s += 1)
                    {
                        template.Add(field[x, y].Value);
                    }
                }
            }
        }
        public List<IFieldCellValue> AddNearAreaToTemplate(List<IFieldCellValue> template, IFieldCellValue[] nearAreaValues)
        {
            for (var i = 0; i < nearAreaValues.Length; i += 1)
            {
                template.Add(nearAreaValues[i]);
            }
            return template;
        }
        #endregion

        private Snake GetRandomSnake(GameField field)
        {
            var head = GetRandomHead(field);
            var body = GetRandomBody(field, head);

            return new Snake(head, body);
        }

        private SnakeHead GetRandomHead(GameField field)
        {
            var randomXHeadPosition = RandomGen.GetRandomX(field.width - 3) + 1;
            var randomYHeadPosition = RandomGen.GetRandomY(field.height - 3) + 1;

            var randomDirection = RandomGen.GetDirection();

            SnakeHead head = new SnakeHead(new FieldCoordinates(randomXHeadPosition, randomYHeadPosition), randomDirection);
            head.Figure = "1";

            return head;
        }
        private SnakeBodyPart GetRandomBody(GameField field, SnakeHead head)
        {
            var x = 0;
            var y = 0;

            switch (head.Direction)
            {
                case "Up":
                    x = head.Position.X;
                    y = head.Position.Y + 1;
                    break;
                case "Down":
                    x = head.Position.X;
                    y = head.Position.Y - 1;
                    break;
                case "Right":
                    x = head.Position.X - 1;
                    y = head.Position.Y;
                    break;
                case "Left":
                    x = head.Position.X + 1;
                    y = head.Position.Y;
                    break;
            }

            var body = new SnakeBodyPart(new FieldCoordinates(x, y));
            body.Figure = "2";
            return body;
        }

        public void ProcessKeyControl(Network network)
        {
            while (network.isTrainingMode)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Spacebar:
                        network.isTrainingMode = false;
                        break;
                    case ConsoleKey.UpArrow:
                        BackTrainer.Speed += 0.01;
                        break;
                    case ConsoleKey.DownArrow:
                        BackTrainer.Speed -= 0.01;
                        break;
                    default:
                        break;
                }
            }
        }


        #endregion

        #region Конструкторы
        public Trainer(NetworkController networkControl, NetworkViewController fieldViewer)
        {
            this.FieldViewer = fieldViewer;
            this.NetworkControl = networkControl;
        }
        #endregion
    }
}
