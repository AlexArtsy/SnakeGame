using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class RenderProcessor
    {
        #region Поля
        #endregion

        #region Свойства
        #endregion

        #region Методы
        public void ClearCell(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(new String(' ', 1));
            Console.SetCursorPosition(x, y);
        }
        public void RenderSnakeMember(SnakeMember member)
        {
            ClearCell(member.X, member.Y);
            Console.ForegroundColor = member.Color;
            Console.BackgroundColor = member.BgColor;
            Console.Write(member.Figure);
        }
        #endregion

        #region Конструкторы
        public RenderProcessor()
        {

        }
        #endregion
    }
}
