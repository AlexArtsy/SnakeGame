using SnakeGame.App.Field;

namespace SnakeGame.App.GameComponents.ViewController
{
    public class NoRendering : IViewer
    {
        public State State { get; set; }
        #region Методы
        public void UpdateField(GameField field)
        {
        }

        public void UpdateFieldCell(FieldCell cell)
        {
        }

        public void BlinkFieldCell(FieldCell cell)
        {
        }

        public void ShowScore()
        {
        }

        public void ShowSpeed()
        {
        }
        #endregion
    }
}
