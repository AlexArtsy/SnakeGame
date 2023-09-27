﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class RandomGen
    {
        #region Поля
        private readonly State state;
        #endregion

        #region Свойства
        #endregion

        #region Методы

        //public static int GetRandomX()
        //{
        //    return new Random().Next(0, state.fieldWidth);
        //}
        //public int GetRandomY()
        //{
        //    return new Random().Next(0, state.fieldHeight);
        //}
        public static string GetDirection()
        {
            string direction = "";

            switch (new Random().Next(1, 5))
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
        public RandomGen(State state)
        {
            this.state = state;
        }
        #endregion
    }
}