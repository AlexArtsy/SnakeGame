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
        #endregion

        #region Свойства
        public Network Network { get; set; }
        public string NetworkName { get; set; }
        public List<DataSet> DataSet { get; set; } = new List<DataSet>();
        //public FieldCell[,] Area { get; set; } = new FieldCell[3, 3];
        //public List<Value> Inputs { get; set; }
        public BackPropagationTrainer BackTrainer { get; set; }
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
        private Network GetNetwork(List<Value> inputs, int[] neuronsInLayer)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{this.NetworkName}.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, JsonSerializer.Serialize(new Network(9, neuronsInLayer)));
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
            double totalError = 1;
            double sum = 0;
            double errorAvg = 0;
            var count = 0;

            SetUpDataSet(field);

            while (totalError > fidelity)
            {
                this.DataSet.ForEach(d =>
                {
                    UpdateInputs(d.InputData);
                    totalError = this.BackTrainer.Train(d.Target);

                    lock (State.ConsoleWriterLock)
                    {
                        Console.SetCursorPosition(0,0);
                        Console.Write("                 ");
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine(totalError);

                        Console.SetCursorPosition(30, 0);
                        Console.Write("                 ");
                        Console.SetCursorPosition(30, 0);
                        Console.WriteLine(errorAvg);
                    }
                    

                    var randomData = CreateRandomEmptinessTemplate(new GameField(field.X, field.Y, field.width, field.height));
                    UpdateInputs(randomData.InputData);
                    totalError = this.BackTrainer.Train(randomData.Target);
                    sum += totalError;
                    if (totalError < 0.07)
                    {
                        //Console.WriteLine($"{d.InputData[0]}  {d.InputData[1]}   {d.InputData[2]}  ");
                        //Console.WriteLine($"{Network.Outputs[0].Double}  {Network.Outputs[1].Double}  {Network.Outputs[2].Double}  ");
                        //Console.WriteLine();
                    }
                    count += 1;

                    errorAvg = sum / count;
                });

            }
            Console.SetCursorPosition(0, 1);
            Console.WriteLine($"Число трерировок: {count}");
            //UpdateDataFile();
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
