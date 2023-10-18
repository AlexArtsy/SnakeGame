namespace SnakeGame.App.Neural.NetworkComponents
{
    internal class Sigmoid : IActivation
    {
        private readonly double sigma;
        public double Calculate(Value x)
        {
            return 1.0 / (1.0 + Math.Exp(-1 * sigma * x.Double));
        }

        public Sigmoid(double sigma)
        {
            this.sigma = sigma;
        }
    }
}
