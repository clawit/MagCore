using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MagCore.Model.Map
{
    public static class MapFactory
    {
        public static IMap Create(string file)
        {
            Map map = null;
            if (File.Exists(file))
            {
                var lines = File.ReadAllLines(file);
                if (lines != null && lines.Length > 3)
                {
                    var line = lines[0].Trim().Replace(" ", string.Empty);
                    if (line.Contains("E="))
                    {
                        var content = line.Replace("E=", string.Empty);
                        if (Int32.TryParse(content, out var edge))
                        {
                            switch (edge)
                            {
                                case 4:
                                    map = new RectMap(file);
                                    
                                    break;
                                default:
                                    break;
                            }
                            map.Load();
                            if (map.Check())
                                return map;
                        }
                    }
                }
            }

            return null;
        }
    }
}
