using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Field;
using SnakeGame.App.GameComponents;
using SnakeGame.App.GameComponents.OperationController;
using SnakeGame.App.GameComponents.ViewController;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Gamer
{
    public class Robot : Gamer, IGamer
    {
        #region Поля
        public int vision = 5;
        #endregion

        #region Свойства
        public Snake Snake { get; set; }
        public GameField Field { get; set; }
        #endregion

        #region Методы
        public void LockDirection()
        {

        }
        public void Play()
        {
            while (this.Game.State.IsSnakeAlive)
            {
                //Control.DirectionListener(RandomGen.GetDirection()); //  ну например
                Thread.Sleep(1000 - this.Game.State.SnakeSpeed);
            }
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public Robot(Game game) : base(game)
        {

        }
        #endregion
    }
}
