﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    internal interface IPlayable
    {
        string Direction { get; set; }

        void ChangeDirection();
    }
}
