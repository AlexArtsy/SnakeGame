namespace SnakeGame.App.SnakeComponents
{
    internal interface ISnakeable
    {
        string Figure { get; set; }
        //int Speed { get; set; }
        ConsoleColor Color { get; set; }
        ConsoleColor BgColor { get; set; }
    }
}
