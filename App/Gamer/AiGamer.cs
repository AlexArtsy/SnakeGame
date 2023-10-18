using SnakeGame.App.Field;
using SnakeGame.App.GameComponents;
using SnakeGame.App.GameComponents.OperationController;
using SnakeGame.App.GameComponents.ViewController;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Gamer
{
    public class AiGamer : Gamer, IGamer
    {
        #region Поля
        #endregion

        #region Свойства
        //public Network Network { get; set; }
        //public Snake Snake { get; set; }
        
        #endregion

        #region Методы

        public void Play()
        {
            this.Game.Run();

            
        }
        //public void RunRendering()
        //{
        //    this.RenderProcessor.RenderGameField(this.Game.GameField);
        //}

        //public void RunControlListener()
        //{
        //    this.Game.GameControl.UpdateHeadDirection(this.Control.DirectionGenerator());
        //}

        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public AiGamer(Game game) : base(game)
        {

        }
        #endregion
    }
}
