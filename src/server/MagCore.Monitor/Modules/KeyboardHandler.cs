using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules
{
    public static class KeyboardHandler
    {
        public static void Update()
        {
            var state = Keyboard.GetState();

            switch (Global.RunState)
            {
                case RunState.Init:
                    var keys = state.GetPressedKeys();
                    if (keys.Length > 0)
                    {
                        var key = keys[0];
                        if (key >= Keys.D1 && key < (Keys)(Keys.D1 + GameListLoader.Games.Count))
                        {
                            var sel = key - Keys.D1;
                            GameLoader.Load(sel);
                        }

                    }
                    break;
                case RunState.Run:
                    break;
                default:
                    break;
            }
        }

    }
}
