using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnakeGame.App.Field;
using SnakeGame.App.SnakeComponents;

namespace SnakeGame.App.Gamer
{
    public interface IGamer
    {
        Snake Snake { get; set; }
        GameField Field { get; set; }
        void Play();
    }
}
