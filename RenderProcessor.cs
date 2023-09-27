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
            Console.SetCursorPosition(x, y);
            Console.Write(new String(' ', 1));
            Console.SetCursorPosition(x, y);
        } 

        public void UpdateFieldCell(FieldCell cell)
        {
            ClearCell(cell.Position.X, cell.Position.Y);
            Console.ForegroundColor = cell.Color;
            Console.BackgroundColor = cell.BgColor;
            Console.Write(cell.Value.Figure);

            Console.SetCursorPosition(Console.WindowWidth - 1, Console.WindowHeight - 1);
        }

        private void SubscribeFieldCellChangingEvent(GameField gameField)
        {
            foreach (var fieldCell in gameField.Field)
            {
                fieldCell.Changed += UpdateFieldCell;
            }
        }
        #endregion

        #region Конструкторы
        public RenderProcessor(GameField field)
        {
            SubscribeFieldCellChangingEvent(field);
        }
        #endregion
    }
}
