using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class RandomGenerator
    {
        #region Поля
        private readonly State state;
        private readonly Random random = new Random();
        #endregion

        #region Свойства
        #endregion

        #region Методы

        public int GetRandomX()
        {
            return this.random.Next(0, state.fieldWidth);
        }
        public int GetRandomY()
        {
            return this.random.Next(0, state.fieldHeight);
        }
        public string GetStartDirection()
        {
            string direction = "";

            switch (this.random.Next(1, 5))
            {
                case 1:
                    direction = "Up";
                    break;
                case 2:
                    direction = "Down";
                    break;
                case 3:
                    direction = "Right";
                    break;
                case 4:
                    direction = "Left";
                    break;
            }

            return direction;
        }
        #endregion

        #region Конструкторы
        public RandomGenerator(State state)
        {
            this.state = state;
        }
        #endregion
    }
}
