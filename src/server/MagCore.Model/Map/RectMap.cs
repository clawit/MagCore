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
                && this.Rows.Count > pos.Y
                && this.Rows[pos.Y].Cells.Count > pos.X)
            {
                cell = this.Rows[pos.Y].Cells[pos.X];
            }
            
            return cell;
        }

        public override List<Cell> GetSiblings(Cell cell)
        {
            List<Cell> cells = new List<Cell>();

            List<Cell> siblings = new List<Cell>();
            siblings.Add(Locate(new Position(cell.Position.X - 1, cell.Position.Y)));
            siblings.Add(Locate(new Position(cell.Position.X + 1, cell.Position.Y)));
            siblings.Add(Locate(new Position(cell.Position.X, cell.Position.Y - 1)));
            siblings.Add(Locate(new Position(cell.Position.X, cell.Position.Y + 1)));

            foreach (Cell sibling in siblings)
            {
                if (sibling != null 
                    && sibling.Type != CellType.Null
                    && sibling.State != CellState.Flicke)
                {
                    cells.Add(sibling);
                }
            }

            return cells;
        }
    }
}
