using System;

namespace MagCore.Monitor
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args != null && args.Length != 0)
            {
                var port = args[0].Trim();
                if (port.StartsWith("-url="))
                {
                    string url = "http://" + port.Replace("-url=", string.Empty) + "/";
                    Modules.ApiReq.SetUrl(url);
                }
            }
            using (var game = new Game())
                game.Run();
        }
    }
}
