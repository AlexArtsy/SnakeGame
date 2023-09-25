﻿using System;
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
        #endregion

        #region Свойства
        #endregion

        #region Методы
        public void Run()
        {

        }
        #endregion

        #region Конструкторы
        public SnakeGame(State state)
        {
            this.state = state;
            this.field = new GameField();
            this.gameControl = new Control(state);
        }
        #endregion
    }
}
