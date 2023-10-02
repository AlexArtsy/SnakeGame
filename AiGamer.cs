using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using NeuroCompote;

namespace SnakeGame
{
    internal class AiGamer : IGamer
    {
        #region Поля
        #endregion

        #region Свойства
        public Network Network { get; set; }
        public Snake.Snake Snake { get; set; }
        public GameField Field { get; set; }
        public List<double> Inputs { get; set; }
        #endregion

        #region Методы

        private void SetInputs()
        {
            for (var i = 0; i < this.Field.width; i += 1)
            {
                for (var j = 0; j < this.Field.height; j += 1)
                {
                    var entity = this.Field.Field[i, j].Value.ToString();
                    switch (entity)
                    {
                        case "SnakeGame.FieldEmptiness":
                            this.Inputs.Add(0);
                            break;
                        case "SnakeGame.SnakeFood":
                            this.Inputs.Add(1);
                            break;
                        default:
                            this.Inputs.Add(-1);
                            break;
                    }
                }
            }
        }

        private string GetDirection()
        {
            var direction = "";
            var index = this.Network.Outputs.FindIndex(output => output == this.Network.Outputs.Max());

            switch (State.HeadDirection)
            {
                case "Up":
                    if (index == 0)
                    {
                        direction = "Left";
                    } 
                    else if (index == 1)
                    {
                        direction = "Up";
                    }
                    else if (index == 2)
                    {
                        direction = "Right";
                    }
                    break;
                case "Down":
                    if (index == 0)
                    {
                        direction = "Right";
                    }
                    else if (index == 1)
                    {
                        direction = "Down";
                    }
                    else if (index == 2)
                    {
                        direction = "Left";
                    }
                    break;
                case "Right":
                    if (index == 0)
                    {
                        direction = "Up";
                    }
                    else if (index == 1)
                    {
                        direction = "Right";
                    }
                    else if (index == 2)
                    {
                        direction = "Down";
                    }
                    break;
                case "Left":
                    if (index == 0)
                    {
                        direction = "Down";
                    }
                    else if (index == 1)
                    {
                        direction = "Left";
                    }
                    else if (index == 2)
                    {
                        direction = "Up";
                    }
                    break;
            }

            return direction;
        }
        public void Play()
        {
            while (State.IsSnakeAlive)
            {
                SetInputs();
                Network.Calculate();

                lock (State.ConsoleWriterLock)
                {
                    Console.SetCursorPosition(30, 2);
                    Console.Write($"Direction: {GetDirection()}");
                    Console.SetCursorPosition(30, 3);
                    Console.Write($"Network.Outputs[0]: {Network.Outputs[0]}");
                    Console.SetCursorPosition(30, 4);
                    Console.Write($"Network.Outputs[1]: {Network.Outputs[1]}");
                    Console.SetCursorPosition(30, 5);
                    Console.Write($"Network.Outputs[2]: {Network.Outputs[2]}");
                }

                switch (GetDirection())
                {
                    case "Left":
                        Control.DirectionListener("Left");
                        break;
                    case "Right":
                        Control.DirectionListener("Right");
                        break;
                    case "Up":
                        Control.DirectionListener("Up");
                        break;
                    case "Down":
                        Control.DirectionListener("Down");
                        break;
                }

            }
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public AiGamer(GameField field)
        {
            this.Field = field;
            this.Inputs = new List<double>();
            SetInputs();
            this.Network = new Network(this.Inputs,  new int[] { 100, 120, 3 });
        }
        #endregion
    }
}
