using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.Snake
{
    internal class SnakeHead : SnakeMember, ISnakeable
    {
        #region Поля
        #endregion

        #region Свойства
        public string Direction { get; set; }
        #endregion

        #region Методы
        //public void EatFood(FieldCell cell)
        //{
        //    cell.Value = this;
        //}

        public void Eat(IFieldCellValue food, FieldCell cell, Snake snake)
        {
            //  Проверить порядок выполнения
            food.Consume(snake, cell);
            cell.Value = this;  
        }
        #endregion

        #region Конструкторы
        public SnakeHead(FieldCoordinates position, string direction) : base(position)
        {
            this.Direction = direction;
        }
        #endregion
    }
}
