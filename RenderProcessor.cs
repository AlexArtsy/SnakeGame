using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.Snake;

namespace SnakeGame
{
    internal class RenderProcessor
    {
        #region Поля
        #endregion

        #region Свойства
        #endregion

        #region Методы
        private void ClearCell(int x, int y)
        {
            lock (State.ConsoleWriterLock)
            {
                Console.SetCursorPosition(x, y);
                Console.Write(new String(' ', 1));
                Console.SetCursorPosition(x, y);
            }
        } 

        public void UpdateFieldCell(FieldCell cell) //  Сделать статическим
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

        public void SubscribeFieldCellChangingEvent(GameField gameField)
        {
            foreach (var fieldCell in gameField.Field)
            {
                fieldCell.Changed += UpdateFieldCell;
                UpdateFieldCell(fieldCell);
            }
        }

        public static void Update(FieldCell cell) 
        {
            lock (State.ConsoleWriterLock)
            {
                var x = cell.Position.X;
                var y = cell.Position.Y;
                Console.SetCursorPosition(x, y);
                Console.Write(new String(' ', 1));
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
        #endregion

        #region Конструкторы
        public RenderProcessor()
        {
           // SubscribeFieldCellChangingEvent(field);
        }
        #endregion
    }
}
