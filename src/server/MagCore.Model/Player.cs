using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public class Player
    {
        public Player(string name, int index, PlayerColor color)
        {
            Name = name;
            Color = color;
            Index = index;
        }
        public string Name { get; set; }
        
        public string Id { get; } = Guid.NewGuid().ToString("N");

        public string Token { get; set; } = Guid.NewGuid().ToString("N");

        public int Energy { get; set; } = 0;

        public PlayerColor Color { get; set; }

        public PlayerState State { get; set; } = PlayerState.Leisure;

        /// <summary>
        /// Player index based on One
        /// </summary>
        public int Index { get; set; }

        public Dictionary<string, Cell> Bases = null;

        public void Reset()
        {
            Energy = 0;
            Bases = new Dictionary<string, Cell>();
        }
        public string ToJson()
        {
            string json = "{{\"Id\":\"{0}\", \"Name\":\"{1}\", \"Token\":\"{2}\", \"Energy\":{3}, \"Color\":{4}, \"State\":{5}, \"Index\":{6}, \"Bases\":[{7}]}}";

            string sBases = string.Empty;
            if (Bases != null && Bases.Count > 0)
            {
                List<string> keys = new List<string>(Bases.Count);
                foreach (string key in Bases.Keys)
                {
                    keys.Add("\"" + key + "\"");
                }
                sBases = string.Join(",", keys);
            }
            
            return string.Format(json, Id, Name, Token, Energy, (int)Color, (int)State, Index, sBases);
        }
    }
}
