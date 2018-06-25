using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
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

        public string ToJson()
        {
            string json = "[{0}]";
            List<string> cells = new List<string>(Count);
            foreach (Cell cell in Cells)
            {
                cells.Add(cell.ToJson());
            }

            json = string.Format(json, string.Join(",", cells));
            return json;
        }
    }
}
