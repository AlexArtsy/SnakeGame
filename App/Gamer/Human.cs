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
    internal class Human : Gamer, IGamer
    {
        #region Поля
        #endregion

        #region Свойства
        #endregion

        #region Методы
        public void Play()
        {
            this.Game.Run();
            //Task rendering = new Task(RunRendering);
            //Task runGame = new Task(this.Game.Run);
            //Task runControl = new Task(RunControlListener);

            //rendering.Start();
            //runGame.Start();
            //runControl.Start();
        }

        //public void RunRendering()
        //{
        //    while (Game.State.IsSnakeAlive)
        //    {
        //        this.RenderProcessor.RenderGameField(this.Game.GameField);

        //        Thread.Sleep(this.Game.State.GameTickTimeValue);    //  Возможно нужно засинхронизировать с потоком игры
        //    }
        //}

        //public void RunControlListener()
        //{
        //    while (Game.State.IsSnakeAlive)
        //    {
        //        this.Game.GameControl.UpdateHeadDirection(this.Control.DirectionGenerator());
        //    }
        //}
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public Human(Game game) : base(game)
        {
 
        }

        #endregion
    }
}
