using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Field;
using SnakeGame.App.Game;

namespace SnakeGame.App.Gamer
{
    internal class Human : Gamer, IGamer
    {
        #region Поля
        #endregion

        #region Свойства
        public Snake.Snake Snake { get; set; }
        public GameField Field { get; set; }
        #endregion

        #region Методы
        public void Play()
        {
            while (State.IsSnakeAlive)
            {
                var pressedKey = Console.ReadKey(true);
                switch (pressedKey.Key)
                {
                    case ConsoleKey.Tab:
                        break;
                    case ConsoleKey.LeftArrow:
                        Control.DirectionListener("Left");
                        break;
                    case ConsoleKey.RightArrow:
                        Control.DirectionListener("Right");
                        break;
                    case ConsoleKey.UpArrow:
                        Control.DirectionListener("Up");
                        break;
                    case ConsoleKey.DownArrow:
                        Control.DirectionListener("Down");
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public Human(GameField field)
        {
            Field = field;
        }
        #endregion
    }
}
