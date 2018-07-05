using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Sdk.Models
{
    public class Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Position Parse(string pos)
        {
            var xy = pos.Split(',');
            if (xy.Length == 2)
            {
                Position p = new Position(Int32.Parse(xy[0]), Int32.Parse(xy[1]));
                return p;
            }
            else
                return null;
        }

        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString();
        }

        public int X { get; set; }
        public int Y { get; set; }

        public List<Position> GetSiblings()
        {
            List<Position> siblings = new List<Position>();
            if (X >= 1)
                siblings.Add(new Position(X - 1, Y));
            siblings.Add(new Position(X + 1, Y));
            if (Y >= 1)
                siblings.Add(new Position(X, Y - 1));
            siblings.Add(new Position(X, Y + 1));

            return siblings;
        }

    }
}
