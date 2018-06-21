using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MagCore.Monitor.Modules.Map
{
    public class RectMapLoader : IMapLoader
    {
        public static Texture2D _bgCell = null;

        public RectMapLoader()
        {

        }

        public void LoadContent(ContentManager content)
        {
            _bgCell = content.Load<Texture2D>("Images/Rect");
        }
        public void Update()
        {

        }

        public void Draw(SpriteBatch sb)
        {

        }
    }
}
