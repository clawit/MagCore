using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules
{
    public static class GameListLoader
    {
        public static List<dynamic> Games = new List<dynamic>();

        private static bool _started = false;

        private static SpriteFont _font = null;

        public static void LoadContent(ContentManager content)
        {
            _font = content.Load<SpriteFont>("Fonts/ListFont");
        }

        public static void Update()
        {
            //init a new thread to refresh the game list
            if (!_started)
            {
                Task.Factory.StartNew(() => {
                    while (Global.RunState == RunState.Init)
                    {
                        var result = ApiReq.CreateReq()
                                        .WithMethod("api/Game", "get")
                                        .GetResult(out string json);
                        if (result == HttpStatusCode.OK)
                        {
                            dynamic array = DynamicJson.Parse(json);
                            if (array.IsArray)
                            {
                                lock (Games)
                                {
                                    Games.Clear();
                                    foreach (var item in array)
                                    {
                                        Games.Add(item);
                                    }
                                }
                            }
                        }

                        Thread.Sleep(2000);
                    }

                }).ContinueWith((task) => {
                    _started = false;
                });
                
                _started = true;
            }

            KeyboardHandler.Update();
        }

        public static void Draw(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();

            for (int i = 0; i < Games.Count; i++)
            {
                var item = Games[i];
                sb.DrawString(_font, item.id.ToString(), 
                    new Vector2(250, 50 * i + 100), Color.Red);
            }

            sb.End();
        }
    }
}
