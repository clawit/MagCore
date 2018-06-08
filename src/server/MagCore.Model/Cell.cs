using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public class Cell
    {
        public Cell(int x, int y)
        {
            Position = new Position(x, y);
            State = CellState.Empty;
            Owner = Guid.Empty;
        }

        public Position Position { get; set; }

        public string Key => Position.ToString();

        public CellState State { get; set; }

        public Guid Owner { get; set; }
    }
}
