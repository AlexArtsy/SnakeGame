using SnakeGame.App.Field;
using SnakeGame.App.Game;
using SnakeGame.App.SnakeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SnakeGame.App.Neural
{
    public class Trainer
    {
        #region Поля

        private bool isTrainingRun;
        #endregion

        #region Свойства
        public Network Network { get; set; }
        public string NetworkName { get; set; }
        public List<DataSet> DataSet { get; set; } = new List<DataSet>();
        public GameField FieldForRendering { get; set; }
        //public FieldCell[,] Area { get; set; } = new FieldCell[3, 3];
        //public List<Value> Inputs { get; set; }
        public BackPropagationTrainer BackTrainer { get; set; }
        public double TotalError { get; set; } = 1;
        public int Count { get; set; } = 0;
        public double AvgError { get; set; } = 1;
        #endregion

        #region Методы

        private void UpdateInputs(List<IFieldCellValue> input)
        {
            var k = 0;
            input.ForEach(i =>
            {
                switch (i.ToString())
                {
                    case "SnakeGame.App.Field.FieldEmptiness":
                        this.Network.Inputs[k].Double = 0;
                        break;
                    case "SnakeGame.App.SnakeComponents.SnakeFood":
                        this.Network.Inputs[k].Double = 1;
                        break;
                    case "SnakeGame.App.SnakeComponents.SnakeBodyPart":
                        this.Network.Inputs[k].Double = -0.5;
                        break;
                    case "SnakeGame.App.SnakeComponents.SnakeHead":
                        this.Network.Inputs[k].Double = 0.5;
                        break;
                    default:
                        this.Network.Inputs[k].Double = -1;
                        break;
                }
                k += 1;
            });
        }
        public Network GetNetwork(string name, int[] neuronsInLayer)
        {
            this.NetworkName = name;

            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{name}.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, JsonSerializer.Serialize(new Network(name,209, neuronsInLayer)));
            }

            var data = File.ReadAllText(path);
            return JsonSerializer.Deserialize<Network>(data);
            //return new Network(9, neuronsInLayer);
        }

        public void UpdateDataFile()
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{this.NetworkName}.txt";
            var data = JsonSerializer.Serialize(Network);
            File.WriteAllText(path, data);
        }

        public void Train(GameField field, double fidelity)
        {
            double sum = 0;

            SetUpDataSet(field);

            State.TrainingMode = true;
            isTrainingRun = true;

            Task keyControl = new Task(ProcessKeyControl);
            keyControl.Start();

            Task progressRendering = new Task(TrainingProgressRendering);
            progressRendering.Start();

            //while (totalError > fidelity & errorAvg > 0.341)
            while (this.AvgError > 0.1 & isTrainingRun)
            {
                this.DataSet.ForEach(d =>
                {
                    UpdateInputs(d.InputData);
                    this.TotalError = this.BackTrainer.Train(d.Target);

                    var randomData = CreateRandomEmptinessTemplate(new GameField(field.X, field.Y, field.width, field.height));
                    UpdateInputs(randomData.InputData);
                    this.TotalError = this.BackTrainer.Train(randomData.Target);
                    sum += this.TotalError;

                    this.Count += 1;

                    this.AvgError = sum / this.Count;
                });

            }

            State.TrainingMode = false;
            isTrainingRun = false;
            UpdateDataFile();
        }

        #region DataSet Management
        public void SetUpDataSet(GameField field)
        {
            FieldEmptiness emptiness = new FieldEmptiness();
            SnakeBodyPart snake = new SnakeBodyPart(new FieldCoordinates(0, 0));
            SnakeHead head = new SnakeHead(new FieldCoordinates(0, 0), "Up");
            FieldWall wall = new FieldWall();
            SnakeFood food = new SnakeFood();

            var referenceLeft = new double[] { 1, 0, 0 };
            var referenceCenter = new double[] { 0, 1, 0 };
            var referenceRight = new double[] { 0, 0, 1 };

            //var templateList = new List<IFieldCellValue[]>();

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
                emptiness, emptiness, wall,
                emptiness, head, wall,
                emptiness, snake, wall,
            };
            AddDataToDataSet(template, referenceLeft, field);

            template = new IFieldCellValue[]
            {
                wall, emptiness, emptiness,
                wall, head, emptiness,
                wall, snake, emptiness,
            };
            AddDataToDataSet(template, referenceRight, field);

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

        public List<IFieldCellValue> AddFieldToTemplate(GameField field, List<IFieldCellValue> template)
        {
            for (var y = 0; y < field.height; y += 1)
            {
                for (var x = 0; x < field.width; x += 1)
                {
                    template.Add(field.Field[x, y].Value);
                }
            }
            return template;
        }
        public List<IFieldCellValue> AddNearAreaToTemplate(List<IFieldCellValue> template, IFieldCellValue[] nearAreaValues)
        {
            for (var i = 0; i < nearAreaValues.Length; i += 1)
            {
                template.Add(nearAreaValues[i]);
            }
            return template;
        }

        public void AddDataToDataSet(IFieldCellValue[] template, double[] reference, GameField field)
        {
            var dataTemplate = new List<IFieldCellValue>();
            dataTemplate = AddNearAreaToTemplate(dataTemplate, template);
            dataTemplate = AddFieldToTemplate(field, dataTemplate);

            this.FieldForRendering = field;

            this.DataSet.Add(new DataSet(dataTemplate, reference));
        }

        public DataSet CreateRandomEmptinessTemplate(GameField field)
        {
            var halfWidth = field.width / 2;
            var halfHeight = field.height / 2;

            FieldEmptiness emptiness = new FieldEmptiness();
            SnakeBodyPart snake = new SnakeBodyPart(new FieldCoordinates(0, 0));
            SnakeHead head = new SnakeHead(new FieldCoordinates(0, 0), "Up");
            SnakeFood food = new SnakeFood();

            var referenceLeft = new double[] { 1, 0, 0 };
            //var referenceCenter = new double[] { 0, 1, 0 };
            var referenceRight = new double[] { 0, 0, 1 };
            var reference = new double[] { };

            var nearAreaTemplate = new IFieldCellValue[]
            {
                emptiness, emptiness, emptiness,
                emptiness, head, emptiness,
                emptiness, snake, emptiness,
            };

            var randomXHeadPosition = RandomGen.GetRandomX(field.width - 2);
            randomXHeadPosition = randomXHeadPosition == 0 ? randomXHeadPosition + 1 : randomXHeadPosition;

            var randomYHeadPosition = RandomGen.GetRandomY(field.height - 2);
            randomYHeadPosition = randomYHeadPosition == 0 ? randomYHeadPosition + 1 : randomYHeadPosition;

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
                randomXFoodPosition = RandomGen.GetRandomX(halfWidth);
                xFoodPosition = "Left";
            }

            if (randomYHeadPosition < halfHeight)
            {
                randomYFoodPosition = RandomGen.GetRandomX(halfHeight) + halfHeight - 1;
                yFoodPosition = "Down";
            }
            else
            {
                randomYFoodPosition = RandomGen.GetRandomX(halfHeight);
                yFoodPosition = "Up";
            }

            field.Field[randomXFoodPosition, randomYFoodPosition].Value = food;

            switch (randomDirection)
            {
                case "Up":
                    field.Field[randomXHeadPosition, randomYHeadPosition + 1].Value = snake;
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
                    field.Field[randomXHeadPosition, randomYHeadPosition - 1].Value = snake;
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
                    field.Field[randomXHeadPosition - 1, randomYHeadPosition].Value = snake;
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
                    field.Field[randomXHeadPosition + 1, randomYHeadPosition].Value = snake;
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

            for (var i = 0; i < nearAreaTemplate.Length; i += 1)
            {
                template.Add(nearAreaTemplate[i]);
            }

            for (var y = 0; y < field.height; y += 1)
            {
                for (var x = 0; x < field.width; x += 1)
                {
                    template.Add(field.Field[x, y].Value);
                }
            }

            return new DataSet(template, reference);
        }
        #endregion

        public void ProcessKeyControl()
        {
            while (isTrainingRun)
            {
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Spacebar:
                        isTrainingRun = false;
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

        public void TrainingProgressRendering()
        {
            while (isTrainingRun)
            {
                lock (State.ConsoleWriterLock)
                {
                    //Console.SetCursorPosition(0, 0);
                    //RenderProcessor.UpdateField(this.FieldForRendering);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;

                    Console.SetCursorPosition(0, 20);
                    Console.WriteLine($"Ошибка за цикл: {this.TotalError}");

                    Console.SetCursorPosition(40, 20);
                    Console.WriteLine($"Средняя ошибка: {this.AvgError}");

                    Console.WriteLine($"Количество циклов обучения: {this.Count}");
                    Console.WriteLine($"Скорость обучения: {this.BackTrainer.Speed}");
                }
                //Thread.Sleep(500);
            }
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public Trainer(int[] neuronsInLayer)
        {
            //Inputs = new List<Value>();
            //InitInputList();

            //this.Network = net;

            //this.BackTrainer = new BackPropagationTrainer(this.Network);

        }
        #endregion
    }
}
