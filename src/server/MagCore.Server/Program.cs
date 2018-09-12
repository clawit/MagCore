using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MagCore.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (args.Any(g => g.Trim().ToLower().Contains("-thread=")))
            {
                string arg = args.SingleOrDefault(g => g.Trim().ToLower().Contains("-thread="));
                string sThread = arg.ToLower().Trim().Replace("-thread=", string.Empty);
                if (Int32.TryParse(sThread, out int thread))
                {
                    Core.Server.MaxThread = thread;
                }
            }

            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(new string[] { })
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    if (args.Any(g => g.Trim().ToLower().Contains("-port=")))
                    {
                        string arg = args.SingleOrDefault(g => g.Trim().ToLower().Contains("-port="));
                        string sPort = arg.ToLower().Trim().Replace("-port=", string.Empty);
                        if (Int32.TryParse(sPort, out int port))
                        {
                            options.Listen(IPAddress.Any, port);
                        }
                    }
                    else
                        options.Listen(IPAddress.Any, 8000);

                })
                .Build();
    }
}
