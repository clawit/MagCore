using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Sdk.Models
{
    public class Map
    {
        public int Edge { get; set; }
        public int Shift { get; set; }
        public int Direction { get; set; }

        public List<Row> Rows { get; set; } = new List<Row>();
    }
}
