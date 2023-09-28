using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class SnakeGame
    {
        #region Поля
        private readonly GameField field;
        private readonly Control gameControl;
        private RenderProcessor rendering;
        private readonly Snake.Snake snake;
        #endregion

        #region Свойства
        #endregion

        #region Методы
        public void Run()
        {
            this.snake.RunSnake();
        }
        #endregion

        #region Конструкторы
        public SnakeGame()
        {
            this.field = new GameField(5, 5, 20, 20);
            this.rendering = new RenderProcessor();
            this.rendering.SubscribeFieldCellChangingEvent(this.field);
            //this.gameControl = new Control(state);

            this.snake = new Snake.Snake(5, 3, this.field, 100);

        }
        #endregion
    }
}
