using SnakeGame.App.Field;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.SnakeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Neural.Training;
using System.Reflection;
using System.Text.Json;

namespace SnakeGame.App.GameComponents.ViewController
{
    public class NetworkTeachController : IViewer
    {//private FieldEmptiness emptiness = new FieldEmptiness();
        //private FieldWall wall = new FieldWall();

        private string previousDirection = "";
        #region Свойства

        public List<DataSet> DataSet { get; set; }
        public State State { get; set; }
        public Network Network { get; set; }
        public IViewer Rendering { get; set; }
        public double[] NetworkInputsVector { get; set; }

        #endregion

        #region Методы
        public void Clear()
        {
            this.Rendering.Clear();
        }
        private List<IFieldCellValue> GetFieldCellValuesVector(GameField field, FieldCell[,] nearArea)
        {
            List<IFieldCellValue> inputs = new List<IFieldCellValue>();

            var k = 0;
            for (var j = 0; j < 3; j += 1)
            {
                for (var i = 0; i < 3; i += 1)
                {
                    for (var s = 0; s < 30; s += 1)
                    {
                        inputs.Add(nearArea[i, j].Value);
                        k += 1;
                    }
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
        public double[] GetNetworkInputsVector(Network network, List<IFieldCellValue> inputs)
        {
            var networkInputsValues = new double[inputs.Count];

            var k = 0;
            inputs.ForEach(i =>
            {
                if (i.GetType().Equals(typeof(FieldEmptiness)))
                {
                    networkInputsValues[k] = 0;
                }
                else if (i.GetType().Equals(typeof(FieldWall)))
                {
                    networkInputsValues[k] = -1;
                }
                else if (i.GetType().Equals(typeof(SnakeHead)))
                {
                    networkInputsValues[k] = -0.5;
                }
                else if (i.GetType().Equals(typeof(SnakeBodyPart)))
                {
                    networkInputsValues[k] = -0.5;
                }
                else if (i.GetType().Equals(typeof(SnakeFood)))
                {
                    networkInputsValues[k] = 1;
                }
                else
                {
                    networkInputsValues[k] = -1;
                }
                k += 1;
            });

            return networkInputsValues;
        }
        public FieldCell[,] ScanArea(FieldCell[,] field, SnakeHead head, string direction)
        {
            var area = new FieldCell[3, 3];

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
        public void UpdateField(GameField field)    //  Получаем обновленное поле
        {
            var snakeHeadOnField = (SnakeHead)GetSnakeHead(field);
            var nearArea = ScanArea(field.Field, snakeHeadOnField, snakeHeadOnField.Direction);
            var fieldCellValuesVector = GetFieldCellValuesVector(field, nearArea);

            this.NetworkInputsVector = GetNetworkInputsVector(this.Network, fieldCellValuesVector);

            var target = new double[] { 0, 0 };

            if (!this.previousDirection.Equals(this.State.HeadDirection))
            {
                target = ConvertDirection(this.previousDirection, this.State.HeadDirection);
            }
            
            this.DataSet.Add(new DataSet(fieldCellValuesVector, target));

            this.Rendering.UpdateField(field);  //  прокидываем отрисовку дальше

            if (!this.State.IsSnakeAlive)
            {
                UpdateDataFile(this.Network, this.DataSet);
            }
        }
        public List<DataSet> ReadDataSetsFromFileOrCreate(string name)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{name}_DataSet.txt.txt";

            if (!File.Exists(path))
            {
                File.Create(path).Close();
                File.WriteAllText(path, JsonSerializer.Serialize(new List<DataSet>()));
            }

            var data = File.ReadAllText(path);
            var dataSet = JsonSerializer.Deserialize<List<DataSet>>(data);
            return dataSet;
        }
        public void UpdateDataFile(Network network, List<DataSet> dataSet)
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{network.Name}_DataSet.txt";
            var data = JsonSerializer.Serialize(dataSet);
            File.WriteAllText(path, data);
        }

        private double[] ConvertDirection(string prevDirection, string direction)
        {
            var target = new double[] { 0, 0 };

            switch (prevDirection)
            {
                case "Up":
                    switch (this.State.HeadDirection)
                    {
                        case "Left":
                            target = new double[] { 1, 0 };
                            break;
                        case "Right":
                            target = new double[] { 0, 1 };
                            break;
                    }
                    break;
                case "Left":
                    switch (this.State.HeadDirection)
                    {
                        case "Up":
                            target = new double[] { 0, 1 };
                            break;
                        case "Down":
                            target = new double[] { 1, 0 };
                            break;
                    }
                    break;
                case "Down":
                    switch (this.State.HeadDirection)
                    {
                        case "Left":
                            target = new double[] { 0, 1 };
                            break;
                        case "Right":
                            target = new double[] { 1, 0 };
                            break;
                    }
                    break;
                case "Right":
                    switch (this.State.HeadDirection)
                    {
                        case "Up":
                            target = new double[] { 1, 0 };
                            break;
                        case "Down":
                            target = new double[] { 0, 1 };
                            break;
                    }
                    break;
            }

            return target;
        }
        private IFieldCellValue GetSnakeHead(GameField field)
        {
            foreach (var fieldCell in field.Field)
            {
                if (fieldCell.GetType().Equals(typeof(SnakeHead)))
                {
                    return fieldCell.Value;
                }
            }

            return new SnakeHead(new FieldCoordinates(0, 0), "Up");
        }
        public void UpdateFieldCell(FieldCell cell)
        {
            this.Rendering.UpdateFieldCell(cell);
        }
        public void BlinkFieldCell(FieldCell cell)
        {
            this.Rendering.BlinkFieldCell(cell);
        }
        public void ShowScore()
        {
            this.Rendering.ShowScore();
        }
        public void ShowSpeed()
        {
            this.Rendering.ShowSpeed();
        }
        #endregion

        public NetworkTeachController(Network network, State state, IViewer rendering)
        {
            this.Network = network;
            this.Rendering = rendering;
            this.State = state;
            this.previousDirection = state.HeadDirection;
            this.DataSet = ReadDataSetsFromFileOrCreate(network.Name);
        }
    }
}
