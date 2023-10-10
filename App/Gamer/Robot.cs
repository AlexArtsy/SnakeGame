using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Field;
using SnakeGame.App.Game;

namespace SnakeGame.App.Gamer
{
    internal class Robot : Gamer, IGamer
    {
        #region Поля
        public int vision = 5;
        #endregion

        #region Свойства
        public Snake.Snake Snake { get; set; }
        public GameField Field { get; set; }
        #endregion

        #region Методы
        public void LockDirection()
        {

        }
        public void Play()
        {
            while (State.IsSnakeAlive)
            {
                Control.DirectionListener(RandomGen.GetDirection()); //  ну например
                Thread.Sleep(1000 - State.SnakeSpeed);
            }
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public Robot(GameField field)
        {
            Field = field;
        }
        #endregion
    }
}
