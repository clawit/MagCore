using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public class Game
    {
        public Game(string id, string map, int state)
        {
            Id = id;
            Map = map;
            State = state;
        }
        public string Id { get; set; }

        public string Map { get; set; }

        public int State { get; set; }
    }
}
