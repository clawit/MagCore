using MagCore.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MagCore.Core
{
    public static class Players
    {
        public static ConcurrentDictionary<string, Player> All = new ConcurrentDictionary<string, Player>();
        
        public static string NewPlayer(string name, PlayerColor color)
        {
            lock (All)
            {
                if (All.Values.Any(p => p.Name == name))
                    return null;
                else
                {
                    var player = new Player(name, All.Count + 1, color);
                    Players.All.TryAdd(player.Id, player);
                    return player.ToJson();
                }
            }
        }

        public static Player Get(string id)
        {
            if (All.ContainsKey(id))
                return All[id];
            else
                return null;
        }

        public static void RemovePlayers()
        {
            lock (All)
            {
                foreach (var id in All.Keys)
                {
                    var player = All[id];
                    if (player.State == PlayerState.Leisure
                        && (DateTime.Now - player.CreatedAt).TotalMinutes > 10 )
                    {
                        All.TryRemove(id, out player);
                    }
                }
            }
        }
    }
}
