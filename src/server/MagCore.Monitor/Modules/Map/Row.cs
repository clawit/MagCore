using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Monitor.Modules.Map
{
    public class Row
    {
        public Row(int row, int count)
        {
            Cells = new List<Cell>(count);
            for (int i = 0; i < count; i++)
            {
                Cells.Add(new Cell(i, row));
            }
        }

        public List<Cell> Cells = null;

        public int Count => Cells.Count;
    }
}
