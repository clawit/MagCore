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

        public int Index { get; set; }

        public string ToJson()
        {
            string json = "{{\"Id\":\"{0}\", \"Name\":\"{1}\", \"Token\":\"{2}\", \"Energy\":{3}, \"Color\":{4}, \"State\":{5}, \"Index\":{6}}}";

            return string.Format(json, Id, Name, Token, Energy, (int)Color, (int)State, Index);
        }
    }
}
