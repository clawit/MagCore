using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules
{
    public static class ScreenHandler
    {
        public static void Reset()
        {
            Global.Graphics.PreferredBackBufferWidth = 800;
            Global.Graphics.PreferredBackBufferHeight = 480;
            Global.Graphics.ApplyChanges();
        }

        public static void ChangeSize(int col, int row)
        {
            Global.Graphics.PreferredBackBufferWidth = col * 16 + 2;
            Global.Graphics.PreferredBackBufferHeight = row * 16 + 2;
            Global.Graphics.ApplyChanges();
        }
    }
}
