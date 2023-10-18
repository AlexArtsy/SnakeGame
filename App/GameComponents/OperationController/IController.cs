using SnakeGame.App.Field;

namespace SnakeGame.App.GameComponents.OperationController
{
    public interface IController
    {
        State State { get; set; }
        void Run();
        string DirectionGenerator();
    }
}
