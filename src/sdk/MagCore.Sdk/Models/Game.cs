using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Sdk.Models
{
    public class Game
    {
        public Game(int row, int col)
        {
            Rows = new List<Row>(row);
            for (int i = 0; i < row; i++)
            {
                Rows.Add(new Row(i, col));
            }
        }

        public string Id { get; set; }
         
        public string Map { get; set; }

        public int State { get; set; }

        public List<Player> Players { get; set; }

        public List<Row> Rows { get; set; }

        public Cell Locate(int x, int y)
        {
            if (x >= 0 && y >= 0
                && Rows != null && Rows.Count > y
                && Rows[y].Cells.Count > x)
            {
                return Rows[y].Cells[x];
            }
            else
                return null;
        }
    }
}
