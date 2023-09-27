﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class SnakeFood : IFieldCell
    {
        #region Поля
        #endregion

        #region Свойства
        public FieldCoordinates Position { get; set; }
        public string Figure { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        public FieldCell Cell { get; set; }
        #endregion

        #region Методы
        public void Consume(IFieldCell head)
        {
            this.Cell.UpdateCell(head);  //  Срабатывает событие перерисовки ячейки.
        }
        #endregion

        #region Конструкторы

        public SnakeFood(GameField field, FieldCoordinates position)
        {
            this.Cell = field.Field[position.X, position.Y];
            this.Position = position;
            this.Figure = "F";
            this.Color = ConsoleColor.White;
            this.BgColor = ConsoleColor.Black;
        }
        #endregion
    }
}
