using SnakeGame.App.Field;

namespace SnakeGame.App.GameComponents
{
    public class State
    {
        #region Поля
        public string HeadDirection = RandomGen.GetDirection();
        public int FoodPiecesValue = 0;
        public bool IsSnakeAlive = false;
        #endregion

        #region Свойства
        //public bool TrainingMode { get; set; } = false;
        //public GameField Field { get; set; }
        public int GameScore { get; set; } = 0;
        public int GameTickTimeValue { get; set; } = 500;
        //public bool TickFront { get; set; } = false;
        public int SnakeSpeed { get; set; }

        #endregion

        #region Методы

        #endregion

        #region Конструкторы
        public State()
        {

        }
        #endregion
    }
}
