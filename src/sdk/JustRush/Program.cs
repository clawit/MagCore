using MagCore.Sdk.Helper;
using MagCore.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JustRush
{
    class Program
    {
        static Player self = null;
        static Map map = null;
        static Game game = null;
        static void Main(string[] args)
        {
            string input = string.Empty;

            ServerHelper.Initialize("http://test.magcore.clawit.com/");
            //ServerHelper.Initialize("http://localhost:6000/");

            Player:
            Console.WriteLine("Enter nickname:");
            input = Console.ReadLine();
            string name = input.Trim();

            Console.WriteLine("Enter color(0~9):");
            input = Console.ReadLine();
            int color = Int32.Parse(input);

            self = PlayerHelper.CreatePlayer(name, color);
            if (self == null)
            {
                Console.WriteLine("Player has already existed with same name. Try to get a new name.");
                goto Player;
            }
            
            string gameId = string.Empty;
            string mapName = string.Empty;
            Console.WriteLine("1: Create a new game");
            Console.WriteLine("2: Join a game");
            input = Console.ReadLine();
            if (input == "1")
            {
                map = MapHelper.GetMap("RectPhone");
                game = new Game(map.Rows.Count, map.Rows[0].Count);
                gameId = GameHelper.CreateGame("RectPhone");

            }
            else
            {
                Console.WriteLine("Game list:");
                List:
                var list = GameHelper.GameList();
                if (list == null || list.Length == 0)
                {
                    Thread.Sleep(1000);
                    goto List;
                }
                else
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        if (list[i].state == 0)
                        {
                            Console.WriteLine("{0} : {1} 地图:{2}", i, list[i].id.ToString(), list[i].map.ToString());
                        }
                        
                    }
                }
                Console.WriteLine("Select a game to join:");
                input = Console.ReadLine();
                if (Int32.TryParse(input.Trim(), out int sel)
                    && sel >= 0 && sel < list.Length)
                {
                    gameId = list[sel].id.ToString();
                    mapName = list[sel].map.ToString();

                    map = MapHelper.GetMap(mapName);
                    game = new Game(map.Rows.Count, map.Rows[0].Count);

                }
                else
                {
                    Console.WriteLine("Select error.");
                    goto List;
                }
            }


            game.Id = gameId;

            if (!GameHelper.JoinGame(gameId, self.Id))
                Console.WriteLine("Join game fail.");
            else
                Console.WriteLine("Join game Ok.");

            PlayerHelper.GetPlayer(ref self);
            Console.WriteLine("Self info updated.");

            GameHelper.GetGame(gameId, ref game);

            RushAttack();
        }

        static void RushAttack()
        {
            //create a thread to protect self 
            Task.Factory.StartNew(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                while (true)
                {
                    PlayerHelper.GetPlayer(ref self);

                    foreach (var pos in self.Bases)
                    {
                        MapHelper.Attack(game.Id, self.Id, pos.X, pos.Y);

                        var siblings = pos.GetSiblings();
                        foreach (var sibling in siblings)
                        {
                            MapHelper.Attack(game.Id, self.Id, sibling.X, sibling.Y);
                        }
                    }

                    Thread.Sleep(3000);
                }
            });

            //another thread to get the newest game info
            Task.Factory.StartNew(() => {
                Thread.CurrentThread.IsBackground = true;

                while (true)
                {
                    GameHelper.GetGame(game.Id, ref game);

                    Thread.Sleep(500);
                }
            });

            //main thread to rush more as much as possible
            while (true)
            {
                if (game.State == 1)
                {
                    foreach (var row in game.Rows)
                    {
                        foreach (var cell in row.Cells)
                        {
                            if (cell.Type != 0 && cell.State != 1
                                && cell.OwnerIndex == self.Index) //means this cell is self's
                            {
                                var siblings = cell.Position.GetSiblings();
                                foreach (var pos in siblings)
                                {
                                    var target = game.Locate(pos.X, pos.Y);
                                    if (target != null && target.Type != 0 && target.State != 1
                                        && target.OwnerIndex != self.Index)
                                    {
                                        MapHelper.Attack(game.Id, self.Id, pos.X, pos.Y);
                                    }
                                }
                            }
                        }
                    }
                }
                else if (game.State > 1)
                {
                    Console.WriteLine("Game over");

                    break;
                }

                Thread.Sleep(500);
            }
        }
    }
}
