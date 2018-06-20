using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules
{
    public static class GameLoader
    {
        private static bool _started = false;

        private static dynamic _game = null;
        public static void Update()
        {
            //init a new thread to refresh the game list
            if (!_started)
            {
                Task.Factory.StartNew(() => {
                    while (Global.RunState == RunState.Run)
                    {

                        Thread.Sleep(1000);
                    }

                }).ContinueWith((task) => {
                    _started = false;
                });

                _started = true;
            }

            KeyboardHandler.Update();
        }

        internal static void Load(int index)
        {
            if (GameListLoader.Games.Count >= index)
            {
                _game = GameListLoader.Games[index];
                Global.RunState = RunState.Run;
            }
            
        }

        public static void Draw(SpriteBatch sb)
        {
            sb.Begin();


            sb.End();
        }

    }
}
