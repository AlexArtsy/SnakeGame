using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SnakeGame.App.Field;
using SnakeGame.App.Game;
using SnakeGame.App.Gamer;
using SnakeGame.App.Neural;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App
{
    public class AiGamer : IGamer
    {
        #region Поля
        #endregion

        #region Свойства
        public Network Network { get; set; }
        public Snake Snake { get; set; }
        public GameField Field { get; set; }
        public FieldCell[,] Area { get; set; } = new FieldCell[3, 3];
        public List<Value> Inputs { get; set; }

        public GameField testField { get; set; } = new GameField(40, 20, 3, 3);
        #endregion

        #region Методы

        private void UpdateInputs()
        {
            var k = 0;
            for (var j = 0; j < 3; j += 1)
            {
                for (var i = 0; i < 3; i += 1)
                {
                    var entity = Area[i, j].Value.ToString();
                    switch (entity)
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
                }
            }
        }

        private void ScanArea()
        {
            var xHead = Snake.head.Position.X;
            var yHead = Snake.head.Position.Y;

            try
            {
                if (this.Snake.head.Direction == "Up")
                {
                    Area[0, 0].Value = Field.Field[xHead - 1, yHead - 1].Value ?? new FieldWall();
                    Area[1, 0].Value = Field.Field[xHead + 0, yHead - 1].Value ?? new FieldWall();
                    Area[2, 0].Value = Field.Field[xHead + 1, yHead - 1].Value ?? new FieldWall();
                    Area[0, 1].Value = Field.Field[xHead - 1, yHead + 0].Value ?? new FieldWall();

                    Area[1, 1].Value = Field.Field[xHead + 0, yHead + 0].Value ?? new FieldWall();

                    Area[2, 1].Value = Field.Field[xHead + 1, yHead + 0].Value ?? new FieldWall();
                    Area[0, 2].Value = Field.Field[xHead - 1, yHead + 1].Value ?? new FieldWall();
                    Area[1, 2].Value = Field.Field[xHead + 0, yHead + 1].Value ?? new FieldWall();
                    Area[2, 2].Value = Field.Field[xHead + 1, yHead + 1].Value ?? new FieldWall();
                }
                else if (this.Snake.head.Direction == "Right")
                {
                    Area[0, 0].Value = Field.Field[xHead + 1, yHead - 1].Value ?? new FieldWall();
                    Area[1, 0].Value = Field.Field[xHead + 1, yHead + 0].Value ?? new FieldWall();
                    Area[2, 0].Value = Field.Field[xHead + 1, yHead + 1].Value ?? new FieldWall();
                    Area[0, 1].Value = Field.Field[xHead + 0, yHead - 1].Value ?? new FieldWall();

                    Area[1, 1].Value = Field.Field[xHead + 0, yHead + 0].Value ?? new FieldWall();

                    Area[2, 1].Value = Field.Field[xHead + 0, yHead + 1].Value ?? new FieldWall();
                    Area[0, 2].Value = Field.Field[xHead - 1, yHead - 1].Value ?? new FieldWall();
                    Area[1, 2].Value = Field.Field[xHead - 1, yHead + 0].Value ?? new FieldWall();
                    Area[2, 2].Value = Field.Field[xHead - 1, yHead + 1].Value ?? new FieldWall();
                }
                else if (this.Snake.head.Direction == "Down")
                {
                    Area[0, 0].Value = Field.Field[xHead + 1, yHead + 1].Value ?? new FieldWall();
                    Area[1, 0].Value = Field.Field[xHead + 0, yHead + 1].Value ?? new FieldWall();
                    Area[2, 0].Value = Field.Field[xHead - 1, yHead + 1].Value ?? new FieldWall();
                    Area[0, 1].Value = Field.Field[xHead + 1, yHead + 0].Value ?? new FieldWall();

                    Area[1, 1].Value = Field.Field[xHead + 0, yHead + 0].Value ?? new FieldWall();

                    Area[2, 1].Value = Field.Field[xHead - 1, yHead + 0].Value ?? new FieldWall();
                    Area[0, 2].Value = Field.Field[xHead + 1, yHead - 1].Value ?? new FieldWall();
                    Area[1, 2].Value = Field.Field[xHead + 0, yHead - 1].Value ?? new FieldWall();
                    Area[2, 2].Value = Field.Field[xHead - 1, yHead - 1].Value ?? new FieldWall();
                }
                else if (this.Snake.head.Direction == "Left")
                {
                    Area[0, 0].Value = Field.Field[xHead - 1, yHead + 1].Value ?? new FieldWall();
                    Area[1, 0].Value = Field.Field[xHead - 1, yHead + 0].Value ?? new FieldWall();
                    Area[2, 0].Value = Field.Field[xHead - 1, yHead - 1].Value ?? new FieldWall();
                    Area[0, 1].Value = Field.Field[xHead + 0, yHead + 1].Value ?? new FieldWall();

                    Area[1, 1].Value = Field.Field[xHead + 0, yHead + 0].Value ?? new FieldWall();

                    Area[2, 1].Value = Field.Field[xHead + 0, yHead - 1].Value ?? new FieldWall();
                    Area[0, 2].Value = Field.Field[xHead + 1, yHead + 1].Value ?? new FieldWall();
                    Area[1, 2].Value = Field.Field[xHead + 1, yHead + 0].Value ?? new FieldWall();
                    Area[2, 2].Value = Field.Field[xHead + 1, yHead - 1].Value ?? new FieldWall();
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                return;
            }

        }

        private string GetDirection()
        {
            var direction = "";
            var outs = new double[] { this.Network.Outputs[0].Double, this.Network.Outputs[1].Double, this.Network.Outputs[2].Double, };
            var index = Array.IndexOf(outs, outs.Max());

            switch (this.Snake.head.Direction)
            {
                case "Up":
                    switch (index)
                    {
                        case 0:
                            direction = "Left";
                            break;
                        case 1:
                            direction = "Up";
                            break;
                        case 2:
                            direction = "Right";
                            break;
                    }
                    break;
                case "Left":
                    switch (index)
                    {
                        case 0:
                            direction = "Down";
                            break;
                        case 1:
                            direction = "Left";
                            break;
                        case 2:
                            direction = "Up";
                            break;
                    }
                    break;
                case "Down":
                    switch (index)
                    {
                        case 0:
                            direction = "Right";
                            break;
                        case 1:
                            direction = "Down";
                            break;
                        case 2:
                            direction = "Left";
                            break;
                    }
                    break;
                case "Right":
                    switch (index)
                    {
                        case 0:
                            direction = "Up";
                            break;
                        case 1:
                            direction = "Right";
                            break;
                        case 2:
                            direction = "Down";
                            break;
                    }
                    break;
            }

            return direction;
        }
        public void Play()
        {
            Task renderTestField = new Task(() =>
            {
                while (true)
                {
                    ScanArea();
                    UpdateInputs();
                    Network.Calculate();
                    var newDirection = GetDirection();
                    Control.DirectionListener(newDirection);

                    lock (State.ConsoleWriterLock)
                    {
                        Console.SetCursorPosition(30, 2);
                        Console.Write($"Current Direction:                  ");
                        Console.SetCursorPosition(30, 2);
                        Console.Write($"Current Direction: {Snake.head.Direction}");
                        Console.SetCursorPosition(30, 3);
                        Console.Write($"Next Direction:                  ");
                        Console.SetCursorPosition(30, 3);
                        Console.Write($"Next Direction: {GetDirection()}");

                        Console.SetCursorPosition(30, 4);
                        Console.Write($"[0]: {Math.Round(Network.Outputs[0].Double, 2)}");
                        Console.SetCursorPosition(40, 4);
                        Console.Write($"[1]: {Math.Round(Network.Outputs[1].Double, 2)}");
                        Console.SetCursorPosition(50, 4);
                        Console.Write($"[2]: {Math.Round(Network.Outputs[2].Double, 2)}");

                        Console.SetCursorPosition(25, 8);
                        Console.Write("Входа нейросети:");

                        Console.SetCursorPosition(25, 19);
                        Console.Write("Что видит змея:");

                        Console.SetCursorPosition(40, 9);
                        Console.Write($"               ");
                        Console.SetCursorPosition(40, 10);
                        Console.Write($"               ");
                        Console.SetCursorPosition(40, 11);
                        Console.Write($"               ");

                        Console.SetCursorPosition(40, 9);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[0].InputValue.Double}");
                        Console.SetCursorPosition(45, 9);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[1].InputValue.Double}");
                        Console.SetCursorPosition(50, 9);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[2].InputValue.Double}");

                        Console.SetCursorPosition(40, 10);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[3].InputValue.Double}");
                        Console.SetCursorPosition(45, 10);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[4].InputValue.Double}");
                        Console.SetCursorPosition(50, 10);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[5].InputValue.Double}");

                        Console.SetCursorPosition(40, 11);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[6].InputValue.Double}");
                        Console.SetCursorPosition(45, 11);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[7].InputValue.Double}");
                        Console.SetCursorPosition(50, 11);
                        Console.Write($"{Network.Layers[0].Neurons[0].Inputs[8].InputValue.Double}");
                    }

                    for (int y = 0; y < 3; y += 1)
                    {
                        for (int x = 0; x < 3; x += 1)
                        {
                            testField.Field[x, y].Value = Area[x, y].Value;
                        }
                    }
                    Thread.Sleep(400);
                }
            });

            renderTestField.Start();


            //while (State.IsSnakeAlive)
            //{
            //    var pressedKey = Console.ReadKey(true);
            //    switch (pressedKey.Key)
            //    {
            //        case ConsoleKey.Tab:
            //            break;
            //        case ConsoleKey.LeftArrow:
            //            Control.DirectionListener("Left");
            //            break;
            //        case ConsoleKey.RightArrow:
            //            Control.DirectionListener("Right");
            //            break;
            //        case ConsoleKey.UpArrow:
            //            Control.DirectionListener("Up");
            //            break;
            //        case ConsoleKey.DownArrow:
            //            Control.DirectionListener("Down");
            //            break;
            //        default:
            //            break;
            //    }

            //}
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public AiGamer(Network net, GameField field)
        {
            this.Field = field;
            Inputs = new List<Value>();
            //InitInputList();


            for (int y = 0; y < 3; y += 1)
            {
                for (int x = 0; x < 3; x += 1)
                {
                    Area[x, y] = new FieldCell(x, y);
                    testField.Field[x, y].Value = Area[x, y].Value;
                }
            }

            //SetInputs();
            this.Network = net;
            //Network = new Network(Inputs, new int[] { 20, 40, 4 });

        }
        #endregion
    }
}
