using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Monitor.Modules
{
    public class Player
    {
        public Player(string name, int index, int color)
        {
            Name = name;
            Color = color;
            Index = index;
        }
        public string Name { get; set; }
        
        public string Id { get; } = Guid.NewGuid().ToString("N");

        public string Token { get; set; } = Guid.NewGuid().ToString("N");

        public int Energy { get; set; } = 0;

        public int Color { get; set; }

        public int Index { get; set; }
    }
}
