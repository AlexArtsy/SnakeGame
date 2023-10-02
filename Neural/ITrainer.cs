using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuroCompote
{
    internal interface ITrainer
    { 
        Network Network { get; set; }
        double[] Inputs { get; set; }
        double[] Reference { get; set; }

        public void Train();
    }
}
