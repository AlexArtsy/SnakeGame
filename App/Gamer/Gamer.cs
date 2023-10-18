using SnakeGame.App.GameComponents;
using SnakeGame.App.GameComponents.OperationController;
using SnakeGame.App.GameComponents.ViewController;

namespace SnakeGame.App.Gamer;

public class Gamer
{
    #region Поля
    #endregion

    #region Свойства
    public Game Game { get; set; }
    #endregion

    #region Методы

    #endregion

    #region Делегаты и события

    #endregion

    #region Конструкторы

    public Gamer(Game game)
    {
        this.Game = game;
    }

    #endregion
}