using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal class State
    {
        #region Поля
        public static string HeadDirection = RandomGen.GetDirection();
        public static readonly object ConsoleWriterLock = new object();
        private static int speed = 0;

        #endregion

        #region Свойства

        public static int SnakeSpeed
        {
            get => speed;
            set
            {
                if (speed >= 1000)
                {
                    speed = 1000;
                }
                else if (speed < 500)
                {
                    speed += 50;
                }
                else if (speed < 800)
                {
                    speed += 25;
                }
                else if (speed < 900)
                {
                    speed += 5;
                }
            }
        }
        public static int Score = 0;
        public static int SnakeLength = 0;
        public static int FoodPiecesValue = 0;
        public static bool IsSnakeAlive = false;
        #endregion

        #region Методы
        public static void KillSnake()
        {
            IsSnakeAlive = false;
        }

        #endregion

        #region Конструкторы
        public State()
        {

        }
        #endregion
    }
}
