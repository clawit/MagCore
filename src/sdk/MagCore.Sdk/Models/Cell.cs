using System.Collections.Generic;

namespace MagCore.Sdk.Models
{
    public class Cell
    {
        public Cell(int x, int y)
        {
            Position = new Position(x, y);
            State = 0;
            OwnerIndex = -1;
            Type = 0;
        }

        public Position Position { get; set; }
        public string Key => Position.ToString();

        public int Type { get; set; }

        public int State { get; set; }

        public int OwnerIndex { get; set; }
    }
}