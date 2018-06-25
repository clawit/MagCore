using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MagCore.Monitor.Modules.Map
{
    public class RectMapLoader : IMapLoader
    {
        private static Texture2D _bgRect = null;
        private static Texture2D _bgBase = null;
        private static Texture2D _bgEmpty = null;

        private List<Row> Rows = new List<Row>();

        private Color _color = Color.White;

        private Position _origin = new Position(0, 0);

        public RectMapLoader()
        {

        }
        public void SetMapData(dynamic data)
        {
            int count = 0;
            foreach (string line in data.Rows)
            {
                Row row = new Row(count, line.Length);
                for (int i = 0; i < line.Length; i++)
                {
                    if (Int32.TryParse(line[i].ToString(), out int type))
                        row.Cells[i].Type = type;
                    else
                        row.Cells[i].Type = 0;
                }
                Rows.Add(row);
                count++;
            }

            _origin.X = (800 - (Rows[0].Count * 16)) / 2;
            _origin.Y = (500 - (Rows.Count * 16)) / 2;
        }

        public void LoadContent(ContentManager content)
        {
            if (_bgRect == null)
                _bgRect = content.Load<Texture2D>("Images/Rect");
            if (_bgEmpty == null)
                _bgEmpty = content.Load<Texture2D>("Images/Empty");
            if (_bgBase == null)
                _bgBase = content.Load<Texture2D>("Images/Base");

            Cell.LoadContent(content);
        }

        public void Draw(SpriteBatch sb, GameTime gt)
        {
            for (int i = 0; i < Rows.Count; i++)
            {
                Row row = Rows[i];
                for (int j = 0; j < row.Count; j++)
                {
                    Cell cell = row.Cells[j];
                    Rectangle rect = new Rectangle(j * 15 + _origin.X, i * 15 + _origin.Y, 16, 16);
                    switch (cell.Type)
                    {
                        case 0:
                            //Null
                            //sb.Draw(_bgEmpty, rect, Color.White);
                            break;
                        case 1:
                            //Cell
                            sb.Draw(_bgRect, rect, Color.White);
                            if (GameLoader.Players != null && GameLoader.Players.Count > 0
                                && cell.OwnerIndex > 0)
                                cell.Draw(sb, rect, GameLoader.Players[cell.OwnerIndex].Color, gt);
                            break;
                        case 2:
                            //Base
                            sb.Draw(_bgBase, rect, Color.White);
                            if (GameLoader.Players != null && GameLoader.Players.Count > 0)
                                cell.Draw(sb, rect, GameLoader.Players[cell.OwnerIndex].Color, gt);
                            break;
                        default:
                            continue;
                    }
                }
            }
        }

        public Cell Locate(int x, int y)
        {
            Cell cell = null;
            if (x >= 0 && y >= 0
                && this.Rows.Count >= y
                && this.Rows[y].Cells.Count >= x)
            {
                cell = this.Rows[y].Cells[x];
            }

            return cell;
        }
    }
}
