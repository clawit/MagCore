using MagCore.Model;
using MagCore.Model.Map;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace MagCore.Core
{
    public static class Server
    {
        private static string _path = string.Empty;

        public static int MaxThread = 2;

        private static Hashtable _games = new Hashtable();

        private static Hashtable _maps = new Hashtable();

        public static void Start(string path)
        {
            _path = path;
            LoadMaps();

            Console.WriteLine("MagCore Server Started.");
        }

        public static string NewGame(string map, int thread)
        {
            map = map.Trim().ToLower();
            if (_maps.ContainsKey(map))
            {
                var game = new Game(_maps[map] as IMap, thread);
                _games.Add(game.Id, game);

                return game.Id;
            }
            else
                return null;
        }

        public static Model.Game[] GameList()
        {
            lock (_games)
            {
                List<Model.Game> games = new List<Model.Game>();
                foreach (Game game in _games.Values)
                {
                    if (game.State == GameState.Wait || game.State == GameState.Playing)
                    {
                        games.Add(new Model.Game(game.Id, game.Map, (int)game.State));
                    }
                }

                return games.ToArray();
            }
        }

        public static bool Join(string gameId, string playerId)
        {
            lock (_games)
            {
                if (_games.ContainsKey(gameId))
                {
                    var game = _games[gameId] as Game;
                    if (game.State == GameState.Wait
                        && !game.Players.ContainsKey(playerId))
                    {
                        if (Players.All.ContainsKey(playerId))
                        {
                            var player = Players.All[playerId];
                            player.Reset();

                            if (game.Players.Values.Cast<Player>().Any(p => p.Color == player.Color))
                                return false;
                            else
                            { 
                                game.JoinGame(player);
                                return true;
                            }
                        }
                    }
                }

                return false;
            }
        }

        public static bool StartGame(string id)
        {
            var game = Game(id);
            if (game != null)
                return game.Start();
            else
                return false;
        }

        public static Game Game(string id)
        {
            lock (_games)
            {
                if (_games.ContainsKey(id))
                    return _games[id] as Game;
                else
                    return null;
            }
        }

        private static void LoadMaps()
        {
            if (Directory.Exists(_path))
            {
                var files = Directory.GetFiles(_path, "*.map", SearchOption.AllDirectories);
                foreach (var file in files)
                {
                    var map = MapFactory.Create(file);
                    _maps.Add(map.Name.Trim().ToLower(), map);
                }
            }
        }

        public static IMap GetMap(string name)
        {
            name = name.Trim().ToLower();
            if (_maps.ContainsKey(name))
                return _maps[name] as IMap;
            else
                return null;
        }

        public static void RemoveGame(string gameId)
        {
            lock (_games)
            {
                if (_games.ContainsKey(gameId))
                {
                    var game = _games[gameId] as Game;
                    foreach (string playerId in game.Players.Keys)
                    {
                        Players.All.TryRemove(playerId, out var p);
                    }

                    _games.Remove(gameId);
                }
            }
        }
    }
}
