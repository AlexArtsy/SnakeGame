﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class FieldEmptiness : IFieldCell
    {
        #region Поля
        #endregion

        #region Свойства
        //public FieldCoordinates Position { get; set; }
        public string Figure { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы
        #endregion

        #region Конструкторы
        //public FieldEmptiness(FieldCoordinates position)  ----- нужна ли тут позиция???
        public FieldEmptiness()
        {
            this.Figure = "*";  //  пока так, для отладки, потом будет пробел
            this.Color = ConsoleColor.Gray;
            this.BgColor = ConsoleColor.Black;
        }
        #endregion
    }
}
