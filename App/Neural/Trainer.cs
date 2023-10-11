using SnakeGame.App.Field;
using SnakeGame.App.Game;
using SnakeGame.App.SnakeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SnakeGame.App.Neural
{
    public class Trainer
    {
        #region Поля
        #endregion

        #region Свойства
        public Network Network { get; set; }
        public string NetworkName { get; set; }
        //public FieldCell[,] Area { get; set; } = new FieldCell[3, 3];
        //public List<Value> Inputs { get; set; }
        public BackPropagationTrainer BackTrainer { get; set; }
        #endregion

        #region Методы
        //public void InitInputList()
        //{
        //    var k = 0;
        //    for (var j = 0; j < 3; j += 1)
        //    {
        //        for (var i = 0; i < 3; i += 1)
        //        {
        //            Inputs.Add(new Value(k, 0.0));
        //            k += 1;
        //        }
        //    }
        //}

        private void UpdateInputs(List<IFieldCellValue> input)
        {
            var k = 0;
            input.ForEach(i =>
            {
                switch (i.ToString())
                {
                    case "SnakeGame.App.Field.FieldEmptiness":
                        this.Network.Inputs[k].Double = 0;
                        break;
                    case "SnakeGame.App.SnakeComponents.SnakeFood":
                        this.Network.Inputs[k].Double = 1;
                        break;
                    default:
                        this.Network.Inputs[k].Double = -1;
                        break;
                }
                k += 1;
            });
        }
        private Network GetNetwork(List<Value> inputs, int[] neuronsInLayer)
        {
            //var dir = Directory.GetCurrentDirectory();
            //var path = @$"{dir}\{this.NetworkName}.txt";

            //if (!File.Exists(path))
            //{
            //    File.Create(path).Close();
            //    File.WriteAllText(path, JsonSerializer.Serialize(new Network(inputs, neuronsInLayer)));
            //}

            //var data = File.ReadAllText(path);
            //return JsonSerializer.Deserialize<Network>(data);
            return new Network(9, neuronsInLayer);
        }

        public void UpdateDataFile()
        {
            var dir = Directory.GetCurrentDirectory();
            var path = @$"{dir}\{this.NetworkName}.txt";
            var data = JsonSerializer.Serialize(Network);
            File.WriteAllText(path, data);
        }

        public void Train(List<DataSet> data, double fidelity)
        {
            double totalError = 10000;

            while (totalError > fidelity)
            {
                data.ForEach(d =>
                {
                    UpdateInputs(d.InputData);
                    totalError = this.BackTrainer.Train(d.Target);
                    //Console.WriteLine(totalError);

                    if (totalError < 0.07)
                    {
                        //Console.WriteLine($"{d.InputData[0]}  {d.InputData[1]}   {d.InputData[2]}  ");
                        //Console.WriteLine($"{Network.Outputs[0].Double}  {Network.Outputs[1].Double}  {Network.Outputs[2].Double}  ");
                        //Console.WriteLine();
                    }
                });

            }

            //UpdateDataFile();
        }
        #endregion

        #region Делегаты и события
        #endregion

        #region Конструкторы

        public Trainer(int[] neuronsInLayer)
        {
            //Inputs = new List<Value>();
            //InitInputList();

            //this.Network = net;

            //this.BackTrainer = new BackPropagationTrainer(this.Network);

        }
        #endregion
    }
}
