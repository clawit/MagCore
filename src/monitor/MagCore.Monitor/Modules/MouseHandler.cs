using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules
{
    public static class MouseHandler
    {
        public static void ShowMouse(Game game)
        {
            game.IsMouseVisible =true;
        }

        public static void HideMouse(Game game)
        {
            game.IsMouseVisible = false;
        }

    }
}
