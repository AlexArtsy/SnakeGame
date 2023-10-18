using System.Xml;
using SnakeGame.App.Field;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.Neural.Training;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.GameComponents.ViewController
{
    internal class ConsoleRendering : IViewer
    {
        #region Поля
        public readonly object ConsoleWriterLock = new object();
        #endregion

        #region Свойства
        public State State { get; set; }
        #endregion

        #region Методы

        public void UpdateField(GameField field)
        {
            foreach (var fieldCell in field.Field)
            {
                if (fieldCell.IsChanged & !fieldCell.IsBlinked)
                {
                    UpdateFieldCell(fieldCell);
                }
                if (fieldCell.IsChanged & fieldCell.IsBlinked)
                {
                    BlinkFieldCell(fieldCell);
                }
            }
        }

        public void UpdateFieldCell(FieldCell cell)
        {
            lock (ConsoleWriterLock)
            {
                Console.SetCursorPosition(cell.Position.X, cell.Position.Y);
                Console.ForegroundColor = cell.Value.Color;
                Console.BackgroundColor = cell.Value.BgColor;
                Console.Write(cell.Value.Figure);

                Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
            }

            cell.IsChanged = false;
        }

        public void BlinkFieldCell(FieldCell cell)
        {
            lock (ConsoleWriterLock)
            {
                var originalColor = cell.Value.BgColor;

                for (var i = 0; i < 3; i += 1)
                {
                    cell.Value.BgColor = cell.BlinkColor;
                    UpdateFieldCell(cell);

                    Thread.Sleep(State.GameTickTimeValue / 3);

                    cell.Value.BgColor = ConsoleColor.Black;
                    UpdateFieldCell(cell);

                    Thread.Sleep(State.GameTickTimeValue / 3);

                }
                cell.Value.BgColor = originalColor;
                UpdateFieldCell(cell);
                cell.IsBlinked = false;
                cell.BlinkColor = ConsoleColor.Black;
            }
        }

        public void ShowScore()
        {
            lock (ConsoleWriterLock)
            {
                Console.SetCursorPosition(3, 3);
                Console.Write($"Очки: {State.GameScore}");
            }
        }

        public void ShowSpeed()
        {
            lock (ConsoleWriterLock)
            {
                Console.SetCursorPosition(3, 2);
                Console.Write($"Скорость: {State.SnakeSpeed}");
            }
        }
        public void RenderDiagnisticInfo(Network network, Snake snake, FieldCell[,] area)
        {
            var field = new GameField(40, 15, 3, 3).Field;
            var outs = new double[] {
                network.Outputs[0].Double + network.Outputs[1].Double ,
                network.Outputs[2].Double + network.Outputs[3].Double + network.Outputs[4].Double,
                network.Outputs[5].Double + network.Outputs[6].Double,
            };

            lock (ConsoleWriterLock)
            {
                Console.SetCursorPosition(30, 2);
                Console.Write($"Current Direction:                  ");
                Console.SetCursorPosition(30, 2);
                Console.Write($"Current Direction: {snake.head.Direction}");

                Console.SetCursorPosition(30, 4);
                Console.Write($"[0]: {Math.Round(outs[0], 2)}");
                Console.SetCursorPosition(40, 4);
                Console.Write($"[1]: {Math.Round(outs[1], 2)}");
                Console.SetCursorPosition(50, 4);
                Console.Write($"[2]: {Math.Round(outs[2], 2)}");

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
        }

        //public void TrainingProgressRendering(Network network, double totalError, BackPropagationTrainer trainer)
        //{
        //    while (network.isTrainingMode)
        //    {
        //        lock (ConsoleWriterLock)
        //        {
        //            //Console.SetCursorPosition(0, 0);
        //            //RenderProcessor.UpdateField(this.FieldForRendering);

        //            Console.ForegroundColor = ConsoleColor.White;
        //            Console.BackgroundColor = ConsoleColor.Black;

        //            Console.SetCursorPosition(0, 20);
        //            Console.WriteLine($"Ошибка за цикл: {totalError}");

        //            Console.SetCursorPosition(40, 20);
        //            Console.WriteLine($"Средняя ошибка: {network.AvgError}");

        //            Console.WriteLine($"Количество циклов обучения: {network.ValueOfLearningCycles}");
        //            Console.WriteLine($"Скорость обучения: {trainer.Speed}");
        //        }
        //        //Thread.Sleep(500);
        //    }
        //}
        ////public static void DiagnosisInfoRendering(Network network)
        ////{
        ////    lock (State.ConsoleWriterLock)
        ////    {
        ////        Console.SetCursorPosition(30, 4);
        ////        Console.Write($"[0]: {Math.Round(network.Outputs[0].Double, 2)}");
        ////        Console.SetCursorPosition(40, 4);
        ////        Console.Write($"[1]: {Math.Round(network.Outputs[1].Double, 2)}");
        ////        Console.SetCursorPosition(50, 4);
        ////        Console.Write($"[2]: {Math.Round(network.Outputs[2].Double, 2)}");
        ////    }

        ////}
        #endregion

        #region Конструкторы
        public ConsoleRendering(State state)
        {
            this.State = state;
        }
        #endregion
    }
}
