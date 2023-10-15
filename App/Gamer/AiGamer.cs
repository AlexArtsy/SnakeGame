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
        public double[] Outs { get; set; }

        //public GameField testField { get; set; } = new GameField(40, 15, 3, 3);
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
                            this.Network.Inputs[k].Double = 10;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeBodyPart":
                            this.Network.Inputs[k].Double = -5;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeHead":
                            this.Network.Inputs[k].Double = -5;
                            break;
                        default:
                            this.Network.Inputs[k].Double = -10;
                            break;
                    }
                    k += 1;
                }
            }

            for (var j = 0; j < this.Field.height; j += 1)
            {
                for (var i = 0; i < this.Field.width; i += 1)
                {
                    var entity = this.Field.Field[i, j].Value.ToString();
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
                            this.Network.Inputs[k].Double = -0.5;
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
            this.Outs = new double[] { 
                this.Network.Outputs[0].Double + this.Network.Outputs[1].Double ,
                this.Network.Outputs[2].Double + this.Network.Outputs[3].Double + this.Network.Outputs[4].Double,
                this.Network.Outputs[5].Double + this.Network.Outputs[6].Double, 
            };
            var index = Array.IndexOf(this.Outs, this.Outs.Max());

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
            Task running = new Task(Run);
            Task renderTestField = new Task(() =>
            {
                while (State.IsSnakeAlive)
                {
                    RenderProcessor.RenderDiagnisticInfo(this.Network, this.Snake, this.Area);
                    Thread.Sleep(400);
                }
            });
            Task manualControl = new Task(ManualControl);

            running.Start();
            renderTestField.Start();
            manualControl.Start();

        }

        private void Run()
        {
            while (State.IsSnakeAlive)
            {
                ScanArea();
                UpdateInputs();
                Network.Calculate();
                var newDirection = GetDirection();
                Control.DirectionListener(newDirection);
            }
        }

        private void ManualControl()
        {
            while (State.IsSnakeAlive)
            {
                var pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Tab:
                        break;
                    case ConsoleKey.LeftArrow:
                        Control.DirectionListener("Left");
                        break;
                    case ConsoleKey.RightArrow:
                        Control.DirectionListener("Right");
                        break;
                    case ConsoleKey.UpArrow:
                        Control.DirectionListener("Up");
                        break;
                    case ConsoleKey.DownArrow:
                        Control.DirectionListener("Down");
                        break;
                    default:
                        break;
                }

            }
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public AiGamer(Network net, GameField field)
        {
            this.Field = field;
            Inputs = new List<Value>();


            for (int y = 0; y < 3; y += 1)
            {
                for (int x = 0; x < 3; x += 1)
                {
                    Area[x, y] = new FieldCell(x, y);
                }
            }
            
            this.Network = net;
        }
        #endregion
    }
}
