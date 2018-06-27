using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Sdk.Models
{
    public class Player
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Token { get; set; }

        public int Energy { get; set; }

        public int Color { get; set; }

        public int State { get; set; }

        public int Index { get; set; }

        public List<Position> Bases { get; set; } = new List<Position>();

        public object Locker = new object();
    }
}
