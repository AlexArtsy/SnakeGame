namespace SnakeGame.App.Neural.NetworkComponents
{
    public class Value
    {
        public int Id { get; set; }
        public double Double { get; set; }  //  для передачи по ссылке примитивного значения

        public Value(int id, double value)
        {
            Id = id;
            Double = value;
        }
    }
}
