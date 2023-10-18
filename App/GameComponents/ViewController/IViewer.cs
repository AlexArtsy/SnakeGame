using SnakeGame.App.Field;

namespace SnakeGame.App.GameComponents.ViewController
{
    public interface IViewer
    {
        #region Поля
        #endregion

        #region Свойства
        State State { get; set; }
        #endregion

        #region Методы

        //public void ClearCell(int x, int y);
        void UpdateField(GameField field);
        void UpdateFieldCell(FieldCell cell);
        void BlinkFieldCell(FieldCell cell);
        void ShowScore();
        void ShowSpeed();
        void Clear();

        #endregion
    }
}
