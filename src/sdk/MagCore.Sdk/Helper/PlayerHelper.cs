using MagCore.Sdk.Common;
using MagCore.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace MagCore.Sdk.Helper
{
    public static class PlayerHelper
    {
        public static Player CreatePlayer(string name, int color)
        {
            string parms = string.Format("{{\"Name\":\"{0}\", \"Color\":{1}}}", name, color);
            var content = new StringContent(parms, Encoding.UTF8, "application/json");
            var code = ApiRequest.CreateRequest()
                        .WithMethod("api/player", "post", content)
                        .GetResult(out string json);
            if (code == System.Net.HttpStatusCode.OK)
            {
                var result = DynamicJson.Parse(json);
                Player player = new Player();
                player.Color = color;
                player.Energy = (int)result.Energy;
                player.Id = result.Id.ToString();
                player.Index = (int)result.Index;
                player.Name = name;
                player.State = (int)result.State;
                player.Token = result.Token.ToString();

                return player;
            }
            else
                return null;
        }

        public static void GetPlayer(ref Player player)
        {
            var code = ApiRequest.CreateRequest()
                        .WithMethod("api/player/" + player.Id, "get")
                        .GetResult(out string json);
            if (code == System.Net.HttpStatusCode.OK)
            {
                var result = DynamicJson.Parse(json);
                lock (player.Locker)
                {
                    player.Energy = (int)result.Energy;
                    player.State = (int)result.State;
                    //parse bases
                    player.Bases.Clear();
                    foreach (string pos in result.Bases)
                    {
                        player.Bases.Add(Position.Parse(pos));
                    }
                }
            }
        }
    }
}
