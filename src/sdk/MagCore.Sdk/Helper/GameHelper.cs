﻿using MagCore.Sdk.Common;
using MagCore.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Sdk.Helper
{
    public static class GameHelper
    {
        public static string CreateGame(string map, Dictionary<string, object> extraArgs = null)
        {
            string parms = string.Format("{{\"Map\":\"{0}\"}}", map);

            if (extraArgs != null && extraArgs.Count > 0)
            {
                var jsonObj = DynamicJson.Parse(parms);

                foreach (var key in extraArgs.Keys)
                {
                    if (key.ToLower().Trim() != "map")
                    {
                        jsonObj.TrySet(key, extraArgs[key]);
                    }
                }
                parms = jsonObj.ToString();
                Console.WriteLine($"Creating game with json {parms}");
            }

            var content = new StringContent(parms, Encoding.UTF8, "application/json");
            var code = ApiRequest.CreateRequest()
                        .WithMethod("api/game", "post", content)
                        .GetResult(out string json);
            if (code == System.Net.HttpStatusCode.OK)
                return json;
            else
                return null;
        }

        public static async Task<string> CreateGameAsync(string map, Dictionary<string, object> extraArgs = null)
        {
            string parms = string.Format("{{\"Map\":\"{0}\"}}", map);
            if (extraArgs != null && extraArgs.Count > 0)
            {
                var jsonObj = DynamicJson.Parse(parms);

                foreach (var key in extraArgs.Keys)
                {
                    if (key.ToLower().Trim() != "map")
                    {
                        jsonObj.TrySet(key, extraArgs[key]);
                    }
                }
                parms = jsonObj.ToString();
                Console.WriteLine($"Creating game with json {parms}");
            }

            var content = new StringContent(parms, Encoding.UTF8, "application/json");
            return await ApiRequest.CreateRequest()
                        .WithMethod("api/game", "post", content)
                        .GetResultAsync();
        }

        public static bool JoinGame(string gameId, string playerId)
        {
            string parms = string.Format("{{\"Game\":\"{0}\", \"Player\":\"{1}\"}}", gameId, playerId);
            var content = new StringContent(parms, Encoding.UTF8, "application/json");
            var code = ApiRequest.CreateRequest()
                        .WithMethod("api/game", "patch", content)
                        .GetResult(out string json);
            if (code == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public static async Task<bool> JoinGameAsync(string gameId, string playerId)
        {
            string parms = string.Format("{{\"Game\":\"{0}\", \"Player\":\"{1}\"}}", gameId, playerId);
            var content = new StringContent(parms, Encoding.UTF8, "application/json");
            try
            {
                await ApiRequest.CreateRequest()
                            .WithMethod("api/game", "patch", content)
                            .GetResultAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool StartGame(string gameId, string playerId)
        {
            string parms = string.Format("{{\"Player\":\"{0}\"}}", playerId);
            var content = new StringContent(parms, Encoding.UTF8, "application/json");

            var code = ApiRequest.CreateRequest()
                        .WithMethod("api/game/" + gameId, "put", content)
                        .GetResult(out string json);
            if (code == System.Net.HttpStatusCode.OK)
                return true;
            else
                return false;
        }

        public static async Task<bool> StartGameAsync(string gameId, string playerId)
        {
            string parms = string.Format("{{\"Player\":\"{0}\"}}", playerId);
            var content = new StringContent(parms, Encoding.UTF8, "application/json");

            try
            {
                await ApiRequest.CreateRequest()
                            .WithMethod("api/game/" + gameId, "put", content)
                            .GetResultAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool GetGame(string gameId, ref Game game)
        {
            var code = ApiRequest.CreateRequest()
                        .WithMethod("api/game/" + gameId, "get")
                        .GetResult(out string json);
            if (code == System.Net.HttpStatusCode.OK)
            {
                var result = DynamicJson.Parse(json);
                game.State = (int)result.State;

                //load players
                int iPlayer = 0;
                if (result != null)
                {
                    foreach (var item in result.Players)
                    {
                        iPlayer++;
                    }
                }

                if (result.Id.ToString() != game.Id
                    || game.Players == null
                    || game.Players.Count != iPlayer)
                {
                    game.Players = new List<Player>(iPlayer);
                    foreach (var item in result.Players)
                    {
                        Player player = new Player();
                        player.Index = (int)item.Index;
                        player.Color = (int)item.Color;
                        player.Name = item.Name.ToString();

                        game.Players.Add(player);
                    }
                }

                //load cells
                foreach (var row in result.Cells)
                {
                    foreach (var item in row)
                    {
                        int x = (int)item.X;
                        int y = (int)item.Y;
                        var cell = game.Locate(x, y);
                        cell.Type = (int)item.Type;
                        cell.State = (int)item.State;
                        cell.OwnerIndex = (int)item.Owner;
                    }
                }

                return true;
            }
            else
                return false;
        }

        public static async Task<Game> GetGameAsync(string gameId, int row, int col)
        {
            try
            {
                var json = await ApiRequest.CreateRequest()
                            .WithMethod("api/game/" + gameId, "get")
                            .GetResultAsync();
                var result = DynamicJson.Parse(json);

                //check result 
                if (result == null || result.Id == null || result.Cells == null)
                    return null;

                Game game = new Game(row, col);

                //load state
                game.State = (int)result.State;

                //load players
                int iPlayer = 0;
                if (result != null)
                {
                    foreach (var item in result.Players)
                    {
                        iPlayer++;
                    }
                }
                else
                    return null;

                if (result.Id.ToString() != game.Id
                    || game.Players == null
                    || game.Players.Count != iPlayer)
                {
                    game.Players = new List<Player>(iPlayer);
                    foreach (var item in result.Players)
                    {
                        Player player = new Player();
                        player.Index = (int)item.Index;
                        player.Color = (int)item.Color;
                        player.Name = item.Name.ToString();

                        game.Players.Add(player);
                    }
                }

                //load cells
                foreach (var line in result.Cells)
                {
                    foreach (var item in line)
                    {
                        int x = (int)item.X;
                        int y = (int)item.Y;
                        var cell = game.Locate(x, y);
                        cell.Type = (int)item.Type;
                        cell.State = (int)item.State;
                        cell.OwnerIndex = (int)item.Owner;
                    }
                }

                return game;
            }
            catch
            {
                return null;
            }
        }

        public static dynamic[] GameList()
        {
            var code = ApiRequest.CreateRequest()
                        .WithMethod("api/game", "get")
                        .GetResult(out string json);
            if (code == System.Net.HttpStatusCode.OK)
            {
                var result = DynamicJson.Parse(json);
                List<dynamic> games = new List<dynamic>();
                foreach (var item in result)
                {
                    games.Add(item);
                }

                return games.ToArray();
            }
            else
                return null;
        }

        public static async Task<dynamic[]> GameListAsync()
        {
            try
            {
                var json = await ApiRequest.CreateRequest()
                            .WithMethod("api/game", "get")
                            .GetResultAsync();

                var result = DynamicJson.Parse(json);
                List<dynamic> games = new List<dynamic>();
                foreach (var item in result)
                {
                    games.Add(item);
                }

                return games.ToArray();
            }
            catch 
            {
                return null;
            }
        }

    }
}
