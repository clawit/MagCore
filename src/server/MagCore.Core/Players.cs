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
                    var player = new Player(name, color);
                    Players.All.TryAdd(player.Id, player);
                    return player.ToJson();
                }
            }
        }
    }
}
