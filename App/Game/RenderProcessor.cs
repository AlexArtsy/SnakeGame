using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Field;
using SnakeGame.App.Neural;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Game
{
    internal class RenderProcessor
    {
        #region Поля
        #endregion

        #region Свойства
        #endregion

        #region Методы
        private static void ClearCell(int x, int y)
        {
            lock (State.ConsoleWriterLock)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(new string(' ', 1));
                Console.SetCursorPosition(x, y);
            }
        }

        public static void UpdateField(GameField field)
        {
            for (var y = 0; y < field.height; y += 1)
            {
                for (var x = 0; x < field.width; x += 1)
                {
                    UpdateFieldCell(field.Field[x,y]);
                }
            }
        }

        public static void UpdateFieldCell(FieldCell cell)
        {
            
            lock (State.ConsoleWriterLock)
            {
                ClearCell(cell.Position.X, cell.Position.Y);
                Console.ForegroundColor = cell.Value.Color;
                Console.BackgroundColor = cell.Value.BgColor;
                Console.Write(cell.Value.Figure);

                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            }
        }

        public static void SubscribeFieldCellChangingEvent(GameField gameField)
        {
            if (!State.TrainingMode)
            {
                for (int y = 0; y < gameField.height; y += 1)
                {
                    for (int x = 0; x < gameField.width; x += 1)
                    {
                        gameField.Field[x, y].Changed += UpdateFieldCell;
                        UpdateFieldCell(gameField.Field[x, y]);
                    }
                }
            }
        }

        public static void Update(FieldCell cell)
        {
            lock (State.ConsoleWriterLock)
            {
                var x = cell.Position.X;
                var y = cell.Position.Y;
                Console.SetCursorPosition(x, y);
                Console.Write(new string(' ', 1));
                Console.SetCursorPosition(x, y);

                Console.ForegroundColor = cell.Value.Color;
                Console.BackgroundColor = cell.Value.BgColor;
                Console.Write(cell.Value.Figure);

                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            }
        }
        public static void Blink(FieldCell cell, ConsoleColor color)
        {
            lock (State.ConsoleWriterLock)
            {
                var originalColor = cell.Value.BgColor;
                for (var i = 0; i < 3; i += 1)
                {
                    cell.Value.BgColor = color;
                    Update(cell);
                    Thread.Sleep(200);
                    cell.Value.BgColor = ConsoleColor.Black;
                    Update(cell);
                    Thread.Sleep(200);

                }
                cell.Value.BgColor = originalColor;
            }
        }

        public static void ShowScore()
        {
            lock (State.ConsoleWriterLock)
            {
                Console.SetCursorPosition(3, 3);
                Console.Write($"Очки: {State.Score}");
            }
        }

        public static void ShowSpeed()
        {
            lock (State.ConsoleWriterLock)
            {
                Console.SetCursorPosition(3, 2);
                Console.Write($"Скорость: {State.SnakeSpeed}");
            }
        }
        public static void RenderDiagnisticInfo(Network network, Snake snake, FieldCell[,] area)
        {
            var field = new GameField(40, 15, 3, 3).Field;

            lock (State.ConsoleWriterLock)
            {
                Console.SetCursorPosition(30, 2);
                Console.Write($"Current Direction:                  ");
                Console.SetCursorPosition(30, 2);
                Console.Write($"Current Direction: {snake.head.Direction}");
                //Console.SetCursorPosition(30, 3);
                //Console.Write($"Next Direction:                  ");
                //Console.SetCursorPosition(30, 3);
                //Console.Write($"Next Direction: {GetDirection()}");

                Console.SetCursorPosition(30, 4);
                Console.Write($"[0]: {Math.Round(network.Outputs[0].Double, 2)}");
                Console.SetCursorPosition(40, 4);
                Console.Write($"[1]: {Math.Round(network.Outputs[1].Double, 2)}");
                Console.SetCursorPosition(50, 4);
                Console.Write($"[2]: {Math.Round(network.Outputs[2].Double, 2)}");

                Console.SetCursorPosition(25, 8);
                Console.Write("Входа нейросети:");

                Console.SetCursorPosition(25, 14);
                Console.Write("Что видит змея:");

                Console.SetCursorPosition(40, 9);
                Console.Write($"               ");
                Console.SetCursorPosition(40, 10);
                Console.Write($"               ");
                Console.SetCursorPosition(40, 11);
                Console.Write($"               ");

                Console.SetCursorPosition(40, 9);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[0].InputValue.Double}");
                Console.SetCursorPosition(45, 9);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[1].InputValue.Double}");
                Console.SetCursorPosition(50, 9);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[2].InputValue.Double}");

                Console.SetCursorPosition(40, 10);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[3].InputValue.Double}");
                Console.SetCursorPosition(45, 10);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[4].InputValue.Double}");
                Console.SetCursorPosition(50, 10);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[5].InputValue.Double}");

                Console.SetCursorPosition(40, 11);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[6].InputValue.Double}");
                Console.SetCursorPosition(45, 11);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[7].InputValue.Double}");
                Console.SetCursorPosition(50, 11);
                Console.Write($"{network.Layers[0].Neurons[0].Synapses[8].InputValue.Double}");
            }


            for (int y = 0; y < 3; y += 1)
            {
                for (int x = 0; x < 3; x += 1)
                {
                    field[x, y].Value = area[x, y].Value;
                }
            }

            foreach (var fieldCell in field)
            {
                UpdateFieldCell(fieldCell);
            }
        }

        public static void TrainingProgressRendering(Network network, double totalError, BackPropagationTrainer trainer)
        {
            while (State.TrainingMode)
            {
                lock (State.ConsoleWriterLock)
                {
                    //Console.SetCursorPosition(0, 0);
                    //RenderProcessor.UpdateField(this.FieldForRendering);

                    Console.ForegroundColor = ConsoleColor.White;
                    Console.BackgroundColor = ConsoleColor.Black;

                    Console.SetCursorPosition(0, 20);
                    Console.WriteLine($"Ошибка за цикл: {totalError}");

                    Console.SetCursorPosition(40, 20);
                    Console.WriteLine($"Средняя ошибка: {network.AvgError}");

                    Console.WriteLine($"Количество циклов обучения: {network.ValueOfLearningCycles}");
                    Console.WriteLine($"Скорость обучения: {trainer.Speed}");
                }
                //Thread.Sleep(500);
            }
        }
        public static void DiagnosisInfoRendering(Network network)
        {
            lock (State.ConsoleWriterLock)
            {
                Console.SetCursorPosition(30, 4);
                Console.Write($"[0]: {Math.Round(network.Outputs[0].Double, 2)}");
                Console.SetCursorPosition(40, 4);
                Console.Write($"[1]: {Math.Round(network.Outputs[1].Double, 2)}");
                Console.SetCursorPosition(50, 4);
                Console.Write($"[2]: {Math.Round(network.Outputs[2].Double, 2)}");
            }

        }
        #endregion

        #region Конструкторы
        public RenderProcessor()
        {
            // SubscribeFieldCellChangingEvent(field);
        }
        #endregion
    }
}
