using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public class Map
    {
        public Size Size { get; set; }

        public string File { get; set; }

        public List<Row> Rows = new List<Row>();
    }
}
