using MagCore.Sdk.Helper;
using System;

namespace JustRush
{
    class Program
    {
        static void Main(string[] args)
        {
            string input = string.Empty;

            ServerHelper.Initialize("http://localhost:6000/");

            Console.WriteLine("Enter nickname:");
            input = Console.ReadLine();
            string name = input.Trim();

            Console.WriteLine("Enter color(0~9):");
            input = Console.ReadLine();
            int color = Int32.Parse(input);

            var player = PlayerHelper.CreatePlayer(name, color);

        }
    }
}
