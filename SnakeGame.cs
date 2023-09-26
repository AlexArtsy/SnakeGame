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
        private readonly State state;
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
            while (true)
            {
                this.snake.Move();
                Thread.Sleep(1000);
            }
        }
        #endregion

        #region Конструкторы
        public SnakeGame(State state)
        {
            this.state = state;
            //this.field = new GameField();
            this.gameControl = new Control(state);

            this.snake = new Snake.Snake(5, 3, state);

        }
        #endregion
    }
}
