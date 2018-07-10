using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MagCore.Monitor
{
    public static class Global
    {
        public static RunState RunState { get; set; } = RunState.Init;

        public static ContentManager Content { get; set; }

        public static GraphicsDeviceManager Graphics { get; set; }
    }
}
