using SnakeGame.App.Field;

namespace SnakeGame.App.GameComponents
{
    public class State
    {
        #region Поля

        private int speed = 50;
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
        public int SnakeSpeed
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
