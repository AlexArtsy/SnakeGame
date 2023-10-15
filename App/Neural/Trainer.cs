using SnakeGame.App.Field;
using SnakeGame.App.Game;
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

namespace SnakeGame.App.Neural
{
    public class Trainer
    {
        #region Поля
        #endregion

        #region Свойства
        public List<DataSet> DataSet { get; set; } = new List<DataSet>();
        public GameField FieldForRendering { get; set; }
        public BackPropagationTrainer BackTrainer { get; set; }
        public double TotalError { get; set; } = 1;
        #endregion

        #region Методы

        private void UpdateInputs(Network network, List<IFieldCellValue> inputs)
        {
            var k = 0;
            inputs.ForEach(i =>
            {
                if( k < 3)
                {
                    switch (i.ToString())
                    {
                        case "SnakeGame.App.Field.FieldEmptiness":
                            network.Inputs[k].Double = 10;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeFood":
                            network.Inputs[k].Double = 10;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeBodyPart":
                            network.Inputs[k].Double = -5;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeHead":
                            network.Inputs[k].Double = -5;
                            break;
                        default:
                            network.Inputs[k].Double = -10;
                            break;
                    }
                    k += 1;
                }
                else
                {
                    switch (i.ToString())
                    {
                        case "SnakeGame.App.Field.FieldEmptiness":
                            network.Inputs[k].Double = 0;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeFood":
                            network.Inputs[k].Double = 1;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeBodyPart":
                            network.Inputs[k].Double = -0.5;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeHead":
                            network.Inputs[k].Double = -0.5;
                            break;
                        default:
                            network.Inputs[k].Double = -1;
                            break;
                    }
                    k += 1;
                }
                
            });
        }

        public void UpdateDataFile(Network network)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{network.Name}.txt";
            var data = JsonSerializer.Serialize(network);
            File.WriteAllText(path, data);
        }

        private void Train(Network network, DataSet data)
        {
            UpdateInputs(network, data.InputData);
            this.TotalError = this.BackTrainer.Train(data.Target);

            network.SumForAvgError += this.TotalError;

            network.ValueOfLearningCycles += 1;

            network.AvgError = network.SumForAvgError / network.ValueOfLearningCycles;

            network.HistoryOfAvgError.Add(network.AvgError);

            //RenderProcessor.DiagnosisInfoRendering(network);
            //Console.ReadKey(true);
        }

        public void Run(Network network, GameField field, double fidelity)
        {
            SetUpManualDataSet(field);

            State.TrainingMode = true;

            Task keyControl = new Task(ProcessKeyControl);
            Task progressRendering = new Task(() => RenderProcessor.TrainingProgressRendering(network, this.TotalError, BackTrainer));
            
            progressRendering.Start();
            keyControl.Start();


            while (network.AvgError > fidelity & State.TrainingMode)
            {
                this.DataSet.ForEach(manualData =>
                {
                    Train(network, manualData);

                    var randomData = CreateRandomTemplate(new GameField(field.X, field.Y, field.width, field.height), network);
                    Train(network, randomData);

                });

                //var randomData = CreateRandomTemplate(new GameField(field.X, field.Y, field.width, field.height), network);
                //Train(network, randomData);
            }

            State.TrainingMode = false;
            UpdateDataFile(network);

            keyControl.Wait();
            progressRendering.Wait();

            //Console.Clear();
        }

        #region DataSet Management
        public void SetUpManualDataSet(GameField field)
        {
            FieldEmptiness emptiness = new FieldEmptiness();
            SnakeBodyPart snake = new SnakeBodyPart(new FieldCoordinates(0, 0));
            SnakeHead head = new SnakeHead(new FieldCoordinates(0, 0), "Up");
            FieldWall wall = new FieldWall();
            SnakeFood food = new SnakeFood();

            var referenceLeftFree = new double[] { 0, 1, 0, 0, 0, 0, 0 };
            var referenceLeftFreeExsactement = new double[] { 1, 1, 0, 0, 0, 0, 0 };
            var referenceCenterFree = new double[] { 0, 0, 0, 1, 0, 0, 0 };
            var referenceCenterExsactement = new double[] { 0, 0, 1, 1, 1, 0, 0 };
            var referenceRightFree = new double[] { 0, 0, 0, 0, 0, 1, 0 };
            var referenceRightExactement = new double[] { 0, 0, 0, 0, 0, 1, 1 };


            #region Движение в пустоте
            var template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceCenterFree, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, snake,
            };
            AddDataToDataSet(template, referenceCenterFree, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, emptiness,
                snake, snake, emptiness,
            };
            AddDataToDataSet(template, referenceCenterFree, field);
            #endregion

            #region Движение к еде

            template = new IFieldCellValue[]
            {
                emptiness, food, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceCenterExsactement, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, food,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRightFree, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, food,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRightExactement, field);

            template = new IFieldCellValue[]
            {
                food, emptiness, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceLeftFree, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                food, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceLeftFreeExsactement, field);

            #endregion

            #region Движение у(вдоль) стены и змеи
            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRightFree, field);

            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };
            AddDataToDataSet(template, referenceLeftFree, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, wall,
                emptiness, head, wall,
                emptiness, snake, wall,
            };
            AddDataToDataSet(template, referenceLeftFree, field);

            template = new IFieldCellValue[]
            {
                wall, emptiness, emptiness,
                wall, head, emptiness,
                wall, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRightFree, field);

            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                wall, head, emptiness,
                wall, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRightExactement, field);

            template = new IFieldCellValue[]
            {
                wall, wall, wall,
                emptiness, head, wall,
                emptiness, snake, wall,
            };
            AddDataToDataSet(template, referenceLeftFreeExsactement, field);

            template = new IFieldCellValue[]
            {
                wall, emptiness, snake,
                wall, head, snake,
                wall, snake, snake,
            };
            AddDataToDataSet(template, referenceCenterExsactement, field);

            template = new IFieldCellValue[]
            {
                snake, emptiness, wall,
                snake, head, wall,
                snake, snake, wall,
            };
            AddDataToDataSet(template, referenceCenterExsactement, field);

            template = new IFieldCellValue[]
            {
                wall, emptiness, emptiness,
                wall, head, snake,
                wall, snake, snake,
            };
            AddDataToDataSet(template, referenceCenterExsactement, field);

            template = new IFieldCellValue[]
            {
                emptiness, emptiness, wall,
                snake, head, wall,
                snake, snake, wall,
            };
            AddDataToDataSet(template, referenceCenterExsactement, field);
            #endregion
        }

        public DataSet CreateRandomTemplate(GameField field, Network network)
        {
            var halfWidth = field.width / 2;
            var halfHeight = field.height / 2;

            FieldEmptiness emptiness = new FieldEmptiness();
            SnakeBodyPart snakeBody = new SnakeBodyPart(new FieldCoordinates(0, 0));
            SnakeHead head = new SnakeHead(new FieldCoordinates(0, 0), "Up");
            head.Figure = "1";
            SnakeFood food = new SnakeFood();

            var snake = new Snake(head, snakeBody);


            var referenceLeftFree = new double[] { 0, 1, 0, 0, 0, 0, 0 };
            //var referenceCenter = new double[] { 0, 1, 0 };
            var referenceRightFree = new double[] { 0, 0, 0, 0, 0, 1, 0 };
            var reference = new double[] { };


            var randomXHeadPosition = RandomGen.GetRandomX(field.width - 3) + 1;
            randomXHeadPosition = randomXHeadPosition == 0 ? randomXHeadPosition + 2 : randomXHeadPosition;

            var randomYHeadPosition = RandomGen.GetRandomY(field.height - 3) + 1;
            randomYHeadPosition = randomYHeadPosition == 0 ? randomYHeadPosition + 2 : randomYHeadPosition;

            field.Field[randomXHeadPosition, randomYHeadPosition].Value = head;

            var randomDirection = RandomGen.GetDirection();

            var randomXFoodPosition = 0;
            var randomYFoodPosition = 0;

            var xFoodPosition = "";
            var yFoodPosition = "";


            if (randomXHeadPosition < halfWidth)
            {
                randomXFoodPosition = RandomGen.GetRandomX(halfWidth) + halfWidth - 1;
                xFoodPosition = "Right";
            }
            else
            {
                randomXFoodPosition = RandomGen.GetRandomX(halfWidth) + 1;
                xFoodPosition = "Left";
            }

            if (randomYHeadPosition < halfHeight)
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

            switch (randomDirection)
            {
                case "Up":
                    field.Field[randomXHeadPosition, randomYHeadPosition + 1].Value = snakeBody;
                    switch (xFoodPosition)
                    {
                        case "Right":
                            reference = referenceRightFree;
                            break;
                        case "Left":
                            reference = referenceLeftFree;
                            break;
                    }
                    break;
                case "Down":
                    field.Field[randomXHeadPosition, randomYHeadPosition - 1].Value = snakeBody;
                    switch (xFoodPosition)
                    {
                        case "Right":
                            reference = referenceLeftFree;
                            break;
                        case "Left":
                            reference = referenceRightFree;
                            break;
                    }
                    break;
                case "Right":
                    field.Field[randomXHeadPosition - 1, randomYHeadPosition].Value = snakeBody;
                    switch (yFoodPosition)
                    {
                        case "Up":
                            reference = referenceLeftFree;
                            break;
                        case "Down":
                            reference = referenceRightFree;
                            break;
                    }
                    break;
                case "Left":
                    field.Field[randomXHeadPosition + 1, randomYHeadPosition].Value = snakeBody;
                    switch (yFoodPosition)
                    {
                        case "Up":
                            reference = referenceRightFree;
                            break;
                        case "Down":
                            reference = referenceLeftFree;
                            break;
                    }
                    break;
            }

            var template = new List<IFieldCellValue>();

            var nearArea = ScanArea(field, randomDirection, randomXHeadPosition, randomYHeadPosition);

            AddFieldToTemplate(template, nearArea, 3, 3);
            AddFieldToTemplate(template, field.Field, field.width, field.height);


            //RenderProcessor.RenderDiagnisticInfo(network, snake, nearArea);
            //RenderProcessor.UpdateField(field);

            return new DataSet(template, reference);
        }

        public void AddDataToDataSet(IFieldCellValue[] template, double[] reference, GameField field)
        {
            var dataTemplate = new List<IFieldCellValue>();
            dataTemplate = AddNearAreaToTemplate(dataTemplate, template);
            AddFieldToTemplate(dataTemplate, field.Field, field.width, field.height);

            //his.FieldForRendering = field;

            this.DataSet.Add(new DataSet(dataTemplate, reference));
        }

        private void AddFieldToTemplate(List<IFieldCellValue>  template, FieldCell[,] field, int width, int height)
        {
            for (var y = 0; y < height; y += 1)
            {
                for (var x = 0; x < width; x += 1)
                {
                    template.Add(field[x, y].Value);
                }
            }
        }
        //public List<IFieldCellValue> AddFieldToTemplate(GameField field, List<IFieldCellValue> template)
        //{
        //    for (var y = 0; y < field.height; y += 1)
        //    {
        //        for (var x = 0; x < field.width; x += 1)
        //        {
        //            template.Add(field.Field[x, y].Value);
        //        }
        //    }
        //    return template;
        //}
        public List<IFieldCellValue> AddNearAreaToTemplate(List<IFieldCellValue> template, IFieldCellValue[] nearAreaValues)
        {
            for (var i = 0; i < nearAreaValues.Length; i += 1)
            {
                template.Add(nearAreaValues[i]);
            }
            return template;
        }
        #endregion

        public void ProcessKeyControl()
        {
            while (State.TrainingMode)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Spacebar:
                        State.TrainingMode = false;
                        break;
                    case ConsoleKey.UpArrow:
                        this.BackTrainer.Speed += 0.01;
                        break;
                    case ConsoleKey.DownArrow:
                        this.BackTrainer.Speed -= 0.01;
                        break;
                    default:
                        break;
                }
            }
        }

        private FieldCell[,] ScanArea(GameField field, string direction, int xHeadPos, int yHeadPos)
        {
            var area = new FieldCell[3, 3];

            for (int y = 0; y < 3; y += 1)
            {
                for (int x = 0; x < 3; x += 1)
                {
                    area[x, y] = new FieldCell(x, y);
                    area[x, y].Value = new FieldEmptiness();
                }
            }

            try
            {
                switch (direction)
                {
                    case "Up":
                        area = GetUpOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                    case "Right":
                        area = GetRightOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                    case "Down":
                        area = GetDownOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                    case "Left":
                        area = GetLeftOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                }

                return area;
            }
            catch (IndexOutOfRangeException ex)
            {
                return area;
            }

        }

        private FieldCell[,] GetUpOrientedNearArea(GameField field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field.Field[x - 1, y - 1].Value ?? new FieldWall();
            area[1, 0].Value = field.Field[x + 0, y - 1].Value ?? new FieldWall();
            area[2, 0].Value = field.Field[x + 1, y - 1].Value ?? new FieldWall();
            area[0, 1].Value = field.Field[x - 1, y + 0].Value ?? new FieldWall();

            area[1, 1].Value = field.Field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field.Field[x + 1, y + 0].Value ?? new FieldWall();
            area[0, 2].Value = field.Field[x - 1, y + 1].Value ?? new FieldWall();
            area[1, 2].Value = field.Field[x + 0, y + 1].Value ?? new FieldWall();
            area[2, 2].Value = field.Field[x + 1, y + 1].Value ?? new FieldWall();

            return area;
        }

        private FieldCell[,] GetDownOrientedNearArea(GameField field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field.Field[x + 1, y + 1].Value ?? new FieldWall();
            area[1, 0].Value = field.Field[x + 0, y + 1].Value ?? new FieldWall();
            area[2, 0].Value = field.Field[x - 1, y + 1].Value ?? new FieldWall();
            area[0, 1].Value = field.Field[x + 1, y + 0].Value ?? new FieldWall();

            area[1, 1].Value = field.Field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field.Field[x - 1, y + 0].Value ?? new FieldWall();
            area[0, 2].Value = field.Field[x + 1, y - 1].Value ?? new FieldWall();
            area[1, 2].Value = field.Field[x + 0, y - 1].Value ?? new FieldWall();
            area[2, 2].Value = field.Field[x - 1, y - 1].Value ?? new FieldWall();

            return area;
        }

        private FieldCell[,] GetRightOrientedNearArea(GameField field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field.Field[x + 1, y - 1].Value ?? new FieldWall();
            area[1, 0].Value = field.Field[x + 1, y + 0].Value ?? new FieldWall();
            area[2, 0].Value = field.Field[x + 1, y + 1].Value ?? new FieldWall();
            area[0, 1].Value = field.Field[x + 0, y - 1].Value ?? new FieldWall();

            area[1, 1].Value = field.Field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field.Field[x + 0, y + 1].Value ?? new FieldWall();
            area[0, 2].Value = field.Field[x - 1, y - 1].Value ?? new FieldWall();
            area[1, 2].Value = field.Field[x - 1, y + 0].Value ?? new FieldWall();
            area[2, 2].Value = field.Field[x - 1, y + 1].Value ?? new FieldWall();

            return area;
        }

        private FieldCell[,] GetLeftOrientedNearArea(GameField field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field.Field[x - 1, y + 1].Value ?? new FieldWall();
            area[1, 0].Value = field.Field[x - 1, y + 0].Value ?? new FieldWall();
            area[2, 0].Value = field.Field[x - 1, y - 1].Value ?? new FieldWall();
            area[0, 1].Value = field.Field[x + 0, y + 1].Value ?? new FieldWall();

            area[1, 1].Value = field.Field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field.Field[x + 0, y - 1].Value ?? new FieldWall();
            area[0, 2].Value = field.Field[x + 1, y + 1].Value ?? new FieldWall();
            area[1, 2].Value = field.Field[x + 1, y + 0].Value ?? new FieldWall();
            area[2, 2].Value = field.Field[x + 1, y - 1].Value ?? new FieldWall();

            return area;
        }

        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public Trainer()
        {

        }
        #endregion
    }
}
