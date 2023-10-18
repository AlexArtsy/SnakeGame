using SnakeGame.App.Field;
using SnakeGame.App.GameComponents.ViewController;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.GameComponents.OperationController
{
    public class NetworkController : IController
    {
        public Network Network { get; set; }
        public State State { get; set; }
        public GameField Field{ get; set; }
        public Snake Snake { get; set; }
        public NetworkViewController FieldViewer { get; set; }

        private string GetDirection(List<Value> outputs)   //  Перепроверить направления
        {
            var direction = "";
            var Outs = new double[] {
                outputs[0].Double + outputs[1].Double ,
                outputs[2].Double + outputs[3].Double + outputs[4].Double,
                outputs[5].Double + outputs[6].Double,
            };
            var index = Array.IndexOf(Outs, Outs.Max());

            switch (Snake.head.Direction)
            {
                case "Up":
                    switch (index)
                    {
                        case 0:
                            direction = "Left";
                            break;
                        case 1:
                            direction = "Up";
                            break;
                        case 2:
                            direction = "Right";
                            break;
                    }
                    break;
                case "Left":
                    switch (index)
                    {
                        case 0:
                            direction = "Down";
                            break;
                        case 1:
                            direction = "Left";
                            break;
                        case 2:
                            direction = "Up";
                            break;
                    }
                    break;
                case "Down":
                    switch (index)
                    {
                        case 0:
                            direction = "Right";
                            break;
                        case 1:
                            direction = "Down";
                            break;
                        case 2:
                            direction = "Left";
                            break;
                    }
                    break;
                case "Right":
                    switch (index)
                    {
                        case 0:
                            direction = "Up";
                            break;
                        case 1:
                            direction = "Right";
                            break;
                        case 2:
                            direction = "Down";
                            break;
                    }
                    break;
            }

            return direction;
        }
       
        public void Run()
        {
            while (this.State.IsSnakeAlive)
            {
                this.State.HeadDirection = DirectionGenerator();
                Thread.Sleep(this.State.GameTickTimeValue - 10);    //  А если ноль?
            }
        }
        public string DirectionGenerator()
        {
            var networkInputsVector = this.FieldViewer.NetworkInputsVector; 
            var networkOutputsVector = this.Network.Calculate(networkInputsVector);
            return this.GetDirection(networkOutputsVector);
        }

        public NetworkController(Network network, State state, GameField gameField, Snake snake, NetworkViewController monitoring)
        {
            this.FieldViewer = monitoring;
            this.Network = network;
            this.State = state;
            this.Field = gameField;
            this.Snake = snake;
        }
    }
}
