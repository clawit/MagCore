using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Monitor.Modules.Map
{
    public static class MapLoaderFactory
    {
        public static IMapLoader CreateLoader(string map)
        {
            map = map.Trim().ToUpper();
            if (map.Contains("RECT"))
            {
                return new RectMapLoader();
            }
            else
                return null;
        }
    }
}
