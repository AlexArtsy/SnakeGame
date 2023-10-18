namespace SnakeGame.App.Neural.NetworkComponents
{
    internal class RndGen
    {
        private static Random rnd = new Random();

        public static double GetWeight()
        {
            return 1 - rnd.NextDouble() * 2;
        }

        public static double AdjustWeight()
        {
            return GetWeight() * 0.01;
        }
    }
}
