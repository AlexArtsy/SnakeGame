using SnakeGame.App.Field;
using SnakeGame.App.GameComponents.ViewController;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.SnakeComponents;
using System;

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
            var outs = new double[] {outputs[0].Double, outputs[1].Double };
            //int index = 0;
            var direction = "";

            lock (Game.ConsoleWriterLock)
            {
                Console.SetCursorPosition(0, 0);
                Console.Write($"{outs[0]}     {outs[1]}");

                Console.SetCursorPosition(50, 0);
                Console.Write($"{this.State.HeadDirection}");
            }

            //Console.ReadKey(true);

            if (Math.Abs(outs[0] - outs[1]) < 0.2)
            {
                return this.State.HeadDirection;
            }
            else if(outs[0] < 0.2 & outs[1] < 0.2)
            {
                return this.State.HeadDirection;
            }
            else if(outs[0] > outs[1])
            {
                return ConvertDirection(0);
            }
            else if (outs[0] < outs[1])
            {
                return ConvertDirection(1);
            }
            //else
            //{
            //    index = 1;
            //}

            return this.State.HeadDirection;
        }

        private string ConvertDirection(int index)
        {
            var direction = "";
            switch (this.State.HeadDirection)
            {
                case "Up":
                    switch (index)
                    {
                        case 0:
                            direction = "Left";
                            break;
                        case 1:
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
                            direction = "Down";
                            break;
                    }
                    break;
            }

            return direction;
        }
        public void Run()
        {
            //while (this.State.IsSnakeAlive)
            //{
            //    this.State.HeadDirection = DirectionGenerator();
            //    //Thread.Sleep(1000);    //  А если ноль?
            //}
            this.State.HeadDirection = DirectionGenerator();
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
