using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model.Map

{
    public interface IMap
    {
        Size Size { get; set; }

        string MapFile { get; set; }

        List<Row> Rows { get; }

        int Edge { get; }

        bool Shift { get; }

        int Direction { get; }

        void Load();
    }
}
