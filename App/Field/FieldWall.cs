﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Field
{
    internal class FieldWall : IFieldCellValue
    {
        #region Поля
        #endregion

        #region Свойства
        public string Figure { get; set; }
        public ConsoleColor Color { get; set; }
        public ConsoleColor BgColor { get; set; }
        #endregion

        #region Методы
        public void Consume(Snake snake, FieldCell cell)
        {
            cell.IsBlinked = true;
            cell.BlinkColor = ConsoleColor.Red;
            snake.Die(cell);
        }
        #endregion

        #region Делегаты
        #endregion

        #region События
        #endregion

        #region Конструкторы
        public FieldWall()
        {
            Figure = "W";
            Color = ConsoleColor.White;
            BgColor = ConsoleColor.Red;
        }
        #endregion
    }
}
