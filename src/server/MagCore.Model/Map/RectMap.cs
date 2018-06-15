using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model.Map
{
    public class RectMap : Map
    {
        public RectMap(string file) : base(file)
        {

        }

        public override bool Check()
        {
            if (Edge != 4 && Shift != false && Direction != 0)
            {
                return false;
            }
            else
                return base.Check();
        }

        public override Cell Locate(Position pos)
        {
            Cell cell = null;
            if (pos.X >= 0 && pos.Y >= 0 
                && this.Rows.Count >= pos.Y
                && this.Rows[pos.Y].Cells.Count >= pos.X)
            {
                cell = this.Rows[pos.Y].Cells[pos.X];
            }
            
            return cell;
        }
    }
}
