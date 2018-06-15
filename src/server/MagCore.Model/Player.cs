using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public class Player
    {
        public Player(string name, PlayerColor color)
        {
            Name = name;
            Color = color;
            Id = Guid.NewGuid().ToString("N");
            Token = Guid.NewGuid().ToString("N");
            Energy = 0;
            State = PlayerState.Leisure;
        }
        public string Name { get; set; }
        
        public string Id { get; set; }

        public string Token { get; set; }

        public int Energy { get; set; }

        public PlayerColor Color { get; set; }

        public PlayerState State { get; set; }

        public string ToJson()
        {
            string json = "{{\"Id\":\"{0}\", \"Name\":\"{1}\", \"Token\":\"{2}\", \"Energy\":{3}, \"Color\":{4}, \"State\":{5}}}";

            return string.Format(json, Id, Name, Token, Energy, (int)Color, (int)State);
        }
    }
}
