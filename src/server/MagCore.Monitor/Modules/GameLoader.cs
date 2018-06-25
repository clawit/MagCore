using MagCore.Monitor.Modules.Map;
using Microsoft.Xna.Framework;
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
                    while (Global.RunState == RunState.Run)
                    {
                        string url = "api/Game/" + _game.id.ToString();
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
            //init players when the game switched
            //or players not inited yet
            //or players num changes (parse players first, and compare number )
            List<Player> players = new List<Player>();
            if (data != null)
            {
                foreach (var item in data.Players)
                {
                    Player player = new Player(item.Name.ToString(), Convert.ToInt32(item.Index), Convert.ToInt32(item.Color));
                    players.Add(player);
                }
            }

            if (data.Id.ToString() != _game.id.ToString() 
                || Players == null
                || Players.Count != players.Count)
            {
                Players = new Dictionary<int, Player>();
                foreach (var player in players)
                {
                    Players.Add(player.Index, player);
                }
            }

            //load cells data
            foreach (var row in data.Cells)
            {
                foreach (var item in row)
                {
                    Cell cell = _map.Locate(Convert.ToInt32(item.X), Convert.ToInt32(item.Y));
                    cell.Type = Convert.ToInt32(item.Type);
                    cell.State = Convert.ToInt32(item.State);
                    cell.OwnerIndex = Convert.ToInt32(item.Owner);
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
            Players = null;
        }


        public static void Draw(SpriteBatch sb, GameTime gt)
        {
            sb.Begin();

            if (_map != null)
                _map.Draw(sb, gt);

            sb.End();
        }

    }
}
