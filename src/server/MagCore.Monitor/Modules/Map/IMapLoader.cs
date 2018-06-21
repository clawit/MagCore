using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules.Map
{
    public interface IMapLoader
    {
        void LoadContent(ContentManager content);
        void Draw(SpriteBatch sb);
    }
}
