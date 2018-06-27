using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Sdk.Models
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

        public List<Cell> Cells { get; set; } = new List<Cell>();

        public int Count => Cells.Count;
    }
}
