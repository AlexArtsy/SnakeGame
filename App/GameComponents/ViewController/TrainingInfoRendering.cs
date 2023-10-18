using SnakeGame.App.Field;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.Neural.Training;

namespace SnakeGame.App.GameComponents.ViewController
{
    public class TrainingInfoRendering : IViewer
    {
        #region Свойства
        public State State { get; set; }
        public Network Network { get; set; }
        public BackPropagationTrainer BTrainer { get; set; }
        #endregion

        #region Методы
        public void Clear()
        {
            Console.Clear();
        }

        public void UpdateField(GameField field)
        {
            Console.SetCursorPosition(0, 0);
            Console.Write($"Число циклов: {this.Network.ValueOfLearningCycles}");

            Console.SetCursorPosition(0, 2);
            Console.Write($"Ошибка обучения: {this.Network.TotalError}");

            Console.SetCursorPosition(0, 4);
            Console.Write($"Средняя ошибка: {this.Network.AvgError}");

            Console.SetCursorPosition(0, 6);
            Console.Write($"Скорость обучения: {this.BTrainer.Speed}");

            Console.SetCursorPosition(0, 8);
            Console.Write($"Шаблон: {this.BTrainer.Reference[0]}");
            Console.SetCursorPosition(0, 9);
            Console.Write($"Шаблон: {this.BTrainer.Reference[1]}");
            //Console.SetCursorPosition(0, 10);
            //Console.Write($"Шаблон: {this.BTrainer.Reference[2]}");
            //Console.SetCursorPosition(0, 11);
            //Console.Write($"Шаблон: {this.BTrainer.Reference[3]}");
            //Console.SetCursorPosition(0, 12);
            //Console.Write($"Шаблон: {this.BTrainer.Reference[4]}");
            //Console.SetCursorPosition(0, 13);
            //Console.Write($"Шаблон: {this.BTrainer.Reference[5]}");
            //Console.SetCursorPosition(0, 14);
            //Console.Write($"Шаблон: {this.BTrainer.Reference[6]}");

            Console.SetCursorPosition(10, 8);
            Console.Write($"{this.Network.Outputs[0].Double}");
            Console.SetCursorPosition(10, 9);
            Console.Write($"{this.Network.Outputs[1].Double}");
            //Console.SetCursorPosition(10, 10);
            //Console.Write($"{this.Network.Outputs[2].Double}");
            //Console.SetCursorPosition(10, 11);
            //Console.Write($"{this.Network.Outputs[3].Double}");
            //Console.SetCursorPosition(10, 12);
            //Console.Write($"{this.Network.Outputs[4].Double}");
            //Console.SetCursorPosition(10, 13);
            //Console.Write($"{this.Network.Outputs[5].Double}");
            //Console.SetCursorPosition(10, 14);
            //Console.Write($"{this.Network.Outputs[6].Double}");
        }

        public void UpdateFieldCell(FieldCell cell)
        {

        }

        public void BlinkFieldCell(FieldCell cell)
        {

        }

        public void ShowScore()
        {

        }

        public void ShowSpeed()
        {

        }
        #endregion

        public TrainingInfoRendering(State state, Network network, BackPropagationTrainer bTrainer)
        {
            this.State = state;
            this.Network = network;
            this.BTrainer = bTrainer;
        }
    }
}
