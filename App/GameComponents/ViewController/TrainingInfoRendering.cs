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
