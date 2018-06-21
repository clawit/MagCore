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
        private Texture2D _bgCell = null;

        private List<Row> Rows = new List<Row>();

        private Color _color = new Color(136, 136, 136, 0.75f);

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

            _origin.X = (800 - (Rows[0].Count * 32)) / 2;
            _origin.Y = (500 - (Rows.Count * 32)) / 2;
        }

        public void LoadContent(ContentManager content)
        {
            _bgCell = content.Load<Texture2D>("Images/Rect");
        }

        public void Draw(SpriteBatch sb)
        {
            DrawBg(sb);
        }

        private void DrawBg(SpriteBatch sb)
        {
            for (int i = 0; i < Rows.Count; i++)
            {
                Row row = Rows[i];
                for (int j = 0; j < row.Count; j++)
                {
                    Cell cell = row.Cells[j];
                    sb.Draw(_bgCell, new Vector2(j * 30 + _origin.X, i * 30 + _origin.Y), _color);
                }
            }
        }

    }
}
