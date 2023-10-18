namespace SnakeGame.App.Neural.NetworkComponents
{
    internal class Axon //  Возможно не нужен, т.к. это и есть синапс, а такая сущность уже есть.
    {
        public double Value { get; set; }

        public Axon(double value)
        {
            Value = value;
        }
    }
}
