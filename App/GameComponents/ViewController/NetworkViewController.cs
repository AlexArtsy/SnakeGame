using SnakeGame.App.Field;
using SnakeGame.App.Neural.NetworkComponents;
using SnakeGame.App.SnakeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame.App.GameComponents.ViewController
{
    public class NetworkViewController : IViewer
    {
        #region Свойства

        public State State { get; set; }
        public Network Network { get; set; }
        public IViewer Rendering { get; set; }
        public double[] NetworkInputsVector { get; set; }
        #endregion

        #region Методы
        public void Clear()
        {

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
        public double[] GetNetworkInputsVector(Network network, List<IFieldCellValue> inputs)
        {
            var networkInputsValues = new double[inputs.Count];

            var k = 0;
            inputs.ForEach(i =>
            {
                if (i.Equals(new FieldEmptiness()))
                {
                    networkInputsValues[k] = 0;
                }
                else if (i.Equals(new SnakeFood()))
                {
                    networkInputsValues[k] = 1;
                }
                else if (i.Equals(new SnakeBodyPart(new FieldCoordinates(0, 0))))
                {
                    networkInputsValues[k] = -0.5;
                }
                else if (i.Equals(new SnakeHead(new FieldCoordinates(0, 0), "Up")))
                {
                    networkInputsValues[k] = -0.5;
                }
                else if (i.Equals(new FieldWall()))
                {
                    networkInputsValues[k] = -1;
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

            this.Rendering.UpdateField(field);  //  прокидываем отрисовку дальше
        }

        private IFieldCellValue GetSnakeHead(GameField field)
        {
            foreach (var fieldCell in field.Field)
            {
                if (fieldCell.Value.Equals(new SnakeHead(new FieldCoordinates(0, 0), "Up")))
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

        public NetworkViewController(Network network, State state, IViewer rendering)
        {
            this.Network = network;
            this.Rendering = rendering;
            this.State = state;
        }
    }
}
