using MagCore.Monitor.Modules.Map;
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

        private static int _state = -1;

        internal static Dictionary<int, Player> Players = null;

        private static IMapLoader _map = null;
        public static void Update()
        {
            //init a new thread to refresh the game list
            if (!_started)
            {
                Task.Factory.StartNew(() => {
                    string url = "api/Game/" + _game.id.ToString();
                    while (Global.RunState == RunState.Run)
                    {
                        string json = ApiReq.CreateReq()
                                        .AddMethod(url, "get")
                                        .GetResult();
                        dynamic data = DynamicJson.Parse(json);
                        LoadGameData(data);

                        Thread.Sleep(1000);
                    }

                }).ContinueWith((task) => {
                    _started = false;
                });

                _started = true;
            }

            KeyboardHandler.Update();
        }

        private static void LoadGameData(dynamic data)
        {
            if (data.Id.ToString() != _game.id.ToString())
            {
                //init players when the game switched
                Players = new Dictionary<int, Player>();
                foreach (var item in data.Players)
                {
                    Player player = new Player(item.Name.ToString(), Convert.ToInt32(item.Index), Convert.ToInt32(item.Color));
                    Players.Add(player.Index, player);
                }
            }
        }

        internal static void Load(int index)
        {
            if (GameListLoader.Games.Count >= index)
            {
                _game = GameListLoader.Games[index];

                string map = _game.map.ToString();
                _map = MapLoaderFactory.CreateLoader(map);
                _map.LoadContent(Global.Content);

                string json = ApiReq.CreateReq()
                                .AddMethod("api/map/" + map, "get")
                                .GetResult();
                _map.SetMapData(DynamicJson.Parse(json));

                Global.RunState = RunState.Run;
            }
        }

        internal static void Unload()
        {
            Global.RunState = RunState.Init;
            _map = null;
            _game = null;
        }


        public static void Draw(SpriteBatch sb)
        {
            sb.Begin();

            if (_map != null)
                _map.Draw(sb);

            sb.End();
        }

    }
}
