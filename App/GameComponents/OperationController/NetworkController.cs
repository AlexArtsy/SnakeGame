using SnakeGame.App.Field;
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
        //public FieldCell[,] Area { get; set; } = new FieldCell[3, 3];
        public double[] Inputs { get; set; }

        public double[] GetNetworkInputsVector(Network network, List<IFieldCellValue> inputs)   //  Переделать проверку типа
        {
            var networkInputsValues = new double[inputs.Count];

            var k = 0;
            inputs.ForEach(i =>
            {
                if (k < 3)
                {
                    switch (i.ToString())
                    {
                        case "SnakeGame.App.Field.FieldEmptiness":
                            networkInputsValues[k] = 10;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeFood":
                            networkInputsValues[k] = 10;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeBodyPart":
                            networkInputsValues[k] = -5;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeHead":
                            networkInputsValues[k] = -5;
                            break;
                        default:
                            networkInputsValues[k] = -10;
                            break;
                    }
                    k += 1;
                }
                else
                {
                    switch (i.ToString())
                    {
                        case "SnakeGame.App.Field.FieldEmptiness":
                            networkInputsValues[k] = 0;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeFood":
                            networkInputsValues[k] = 1;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeBodyPart":
                            networkInputsValues[k] = -0.5;
                            break;
                        case "SnakeGame.App.SnakeComponents.SnakeHead":
                            networkInputsValues[k] = -0.5;
                            break;
                        default:
                            networkInputsValues[k] = -1;
                            break;
                    }
                    k += 1;
                }
            });

            return networkInputsValues;
        }
        private List<IFieldCellValue> GetFieldCellValuesVector(GameField field, FieldCell[,] nearArea)
        {
            List<IFieldCellValue> inputs = new List<IFieldCellValue>();

            var k = 0;
            for (var j = 0; j < 3; j += 1)
            {
                for (var i = 0; i < 3; i += 1)
                {
                    inputs.Add(nearArea[i, j].Value);
                    k += 1;
                }
            }

            for (var j = 0; j < field.height; j += 1)
            {
                for (var i = 0; i < field.width; i += 1)
                {
                    inputs.Add(field.Field[i, j].Value);
                    k += 1;
                }
            }

            return inputs;
        }
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
        public FieldCell[,] ScanArea(FieldCell[,] field, SnakeHead head, string direction)
        {
            var area  = new FieldCell[3, 3];

            var xHeadPos = head.Position.X;
            var yHeadPos = head.Position.Y;

            for (int y = 0; y < 3; y += 1)
            {
                for (int x = 0; x < 3; x += 1)
                {
                    area[x, y] = new FieldCell(x, y);
                    area[x, y].Value = new FieldEmptiness();
                }
            }

            try
            {
                switch (direction)
                {
                    case "Up":
                        area = GetUpOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                    case "Right":
                        area = GetRightOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                    case "Down":
                        area = GetDownOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                    case "Left":
                        area = GetLeftOrientedNearArea(field, area, xHeadPos, yHeadPos);
                        break;
                }

                return area;
            }
            catch (IndexOutOfRangeException ex)
            {
                return area;
            }
        }

        private FieldCell[,] GetUpOrientedNearArea(FieldCell[,] field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field[x - 1, y - 1].Value ?? new FieldWall();
            area[1, 0].Value = field[x + 0, y - 1].Value ?? new FieldWall();
            area[2, 0].Value = field[x + 1, y - 1].Value ?? new FieldWall();
            area[0, 1].Value = field[x - 1, y + 0].Value ?? new FieldWall();

            area[1, 1].Value = field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field[x + 1, y + 0].Value ?? new FieldWall();
            area[0, 2].Value = field[x - 1, y + 1].Value ?? new FieldWall();
            area[1, 2].Value = field[x + 0, y + 1].Value ?? new FieldWall();
            area[2, 2].Value = field[x + 1, y + 1].Value ?? new FieldWall();

            return area;
        }
        private FieldCell[,] GetDownOrientedNearArea(FieldCell[,] field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field[x + 1, y + 1].Value ?? new FieldWall();
            area[1, 0].Value = field[x + 0, y + 1].Value ?? new FieldWall();
            area[2, 0].Value = field[x - 1, y + 1].Value ?? new FieldWall();
            area[0, 1].Value = field[x + 1, y + 0].Value ?? new FieldWall();

            area[1, 1].Value = field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field[x - 1, y + 0].Value ?? new FieldWall();
            area[0, 2].Value = field[x + 1, y - 1].Value ?? new FieldWall();
            area[1, 2].Value = field[x + 0, y - 1].Value ?? new FieldWall();
            area[2, 2].Value = field[x - 1, y - 1].Value ?? new FieldWall();

            return area;
        }
        private FieldCell[,] GetRightOrientedNearArea(FieldCell[,] field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field[x + 1, y - 1].Value ?? new FieldWall();
            area[1, 0].Value = field[x + 1, y + 0].Value ?? new FieldWall();
            area[2, 0].Value = field[x + 1, y + 1].Value ?? new FieldWall();
            area[0, 1].Value = field[x + 0, y - 1].Value ?? new FieldWall();

            area[1, 1].Value = field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field[x + 0, y + 1].Value ?? new FieldWall();
            area[0, 2].Value = field[x - 1, y - 1].Value ?? new FieldWall();
            area[1, 2].Value = field[x - 1, y + 0].Value ?? new FieldWall();
            area[2, 2].Value = field[x - 1, y + 1].Value ?? new FieldWall();

            return area;
        }
        private FieldCell[,] GetLeftOrientedNearArea(FieldCell[,] field, FieldCell[,] area, int x, int y)
        {
            area[0, 0].Value = field[x - 1, y + 1].Value ?? new FieldWall();
            area[1, 0].Value = field[x - 1, y + 0].Value ?? new FieldWall();
            area[2, 0].Value = field[x - 1, y - 1].Value ?? new FieldWall();
            area[0, 1].Value = field[x + 0, y + 1].Value ?? new FieldWall();

            area[1, 1].Value = field[x + 0, y + 0].Value ?? new FieldWall();

            area[2, 1].Value = field[x + 0, y - 1].Value ?? new FieldWall();
            area[0, 2].Value = field[x + 1, y + 1].Value ?? new FieldWall();
            area[1, 2].Value = field[x + 1, y + 0].Value ?? new FieldWall();
            area[2, 2].Value = field[x + 1, y - 1].Value ?? new FieldWall();

            return area;
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
            var nearArea = ScanArea(this.Field.Field, this.Snake.head, this.Snake.head.Direction);
            var fieldCellValuesVector = GetFieldCellValuesVector(this.Field, nearArea);
            var networkInputsVector = GetNetworkInputsVector(this.Network, fieldCellValuesVector);
            var networkOutputsVector = this.Network.Calculate(networkInputsVector);
            return this.GetDirection(networkOutputsVector);
        }

        public NetworkController(Network network, State state, GameField gameField, Snake snake)
        {
            this.Network = network;
            this.State = state;
            this.Field = gameField;
            this.Snake = snake;
        }
    }
}
