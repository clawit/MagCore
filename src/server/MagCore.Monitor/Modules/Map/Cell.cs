using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MagCore.Monitor.Modules.Map
{
    public class Cell
    {
        public Cell(int x, int y)
        {
            Position = new Position(x, y);
            State = 0;
            Owner = Guid.Empty;
        }

        public Position Position { get; set; }

        public string Key => Position.ToString();

        public int Type { get; set; }

        public int State { get; set; }

        public Guid Owner { get; set; }

        public DateTime? OccupiedTime { get; set; } = null;
    }
}
