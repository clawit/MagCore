using MagCore.Model.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public class Position
    {
        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }
        public int X { get; set; }
        public int Y { get; set; }

        public override string ToString()
        {
            return X.ToString() + "," + Y.ToString();
        }

        public static bool TryParse(string s, out Position pos)
        {
            pos = null;
            try
            {

                var splits = s.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                if (splits != null && splits.Length == 2)
                {
                    int x = -1, y = -1;
                    if (Int32.TryParse(splits[0].Trim(), out x)
                        && Int32.TryParse(splits[1].Trim(), out y))
                    {
                        pos = new Position(x, y);
                        return true;
                    }
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
