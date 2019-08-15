using MagCore.Model;
using MagCore.Model.Map;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MagCore.Core
{
    public class Game
    {
        public string Id { get; } = Guid.NewGuid().ToString("N");
        private IMap _map { get; set; }
        public string Map => _map.Name;

        public int ThreadNum = Server.MaxThread;

        private GameState _state;
        public GameState State { get { return _state; } }

        private ConcurrentQueue<Command> _commands = new ConcurrentQueue<Command>();

        internal Hashtable Players { get; } = new Hashtable();

        internal string _MainThreadName { get; set; }
        internal string _EnergyThreadName { get; set; }

        internal DateTime CreateTime { get; set; } = DateTime.Now;

        public string Owner { get; set; }

        public string FeedbackUrl { get; set; }

        public byte[] GameCode = null;

        public string ToJson()
        {
            string json = "{{\"Id\":\"{0}\", \"Map\":\"{1}\", \"State\":{2}, \"Players\":[{3}], \"Cells\":{4}}}";
            string players = string.Empty;
            lock (Players)
            {
                if (Players.Count > 0)
                {
                    List<string> sPlayers = new List<string>(Players.Count); 
                    foreach (Player player in Players.Values)
                    {
                        //sPlayers.Add("\"" + player.Index + "\"");
                        string single = "{{\"Index\":{0}, \"Color\":{1}, \"Name\":\"{2}\", \"State\":{3}}}";
                        single = string.Format(single, player.Index, (int)player.Color, player.Name, (int)player.State);
                        sPlayers.Add(single);
                    }
                    players = string.Join(",", sPlayers);
                }
                
            }

            return string.Format(json, Id, _map.Name, (int)State, players, _map.Cells());
        }

        public Game(IMap map, int thread)
        {
            _state = GameState.Wait;
            _map = map.Clone();

            if (thread <= 0 || thread > Server.MaxThread)
                ThreadNum = Server.MaxThread;
            else
                ThreadNum = thread;

            Task.Factory.StartNew(() => {
                _MainThreadName = "GameMainThread." + Thread.CurrentThread.ManagedThreadId.ToString();
                while (_state == GameState.Wait)
                {
                    var ts = DateTime.Now - CreateTime;

                    if (ts.TotalMinutes > 10)
                    {
                        _state = GameState.Done;
                    }
                    else
                        Thread.Sleep(1000);
                }
                try
                {
                    while (_state == GameState.Playing)
                    {
                        if (!_commands.IsEmpty && _commands.TryDequeue(out var cmd))
                        {
                            switch (cmd.Action)
                            {
                                case Model.Action.Attack:
                                    ProcessAttack(cmd, _map);
                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            Thread.Sleep(100);
                        }

                        ProcessVictory();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                }
            }).ContinueWith((task) => {
                //
                if (!string.IsNullOrEmpty(FeedbackUrl))
                {
                    var client = new HttpClient();
                    client.Timeout = TimeSpan.FromSeconds(10);
                    string json = this.ToJson();
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            var response = client.PostAsync(FeedbackUrl, content);
                            if (response.Result.StatusCode != System.Net.HttpStatusCode.OK)
                            {
                                Thread.Sleep(3000);
                                Console.WriteLine($"Failed to post game score to feedback[{FeedbackUrl}] for {i} time(s).");
                                Console.WriteLine($"{json}");
                            }
                        }
                        catch 
                        {
                            Thread.Sleep(3000);
                            Console.WriteLine($"Failed to post game score to feedback[{FeedbackUrl}] for {i} time(s).");
                            Console.WriteLine($"{json}");
                            continue;
                        }
                    }
                }
                
                //Recycling
                _state = GameState.Recycling;
                Thread.Sleep(10000);
                Server.RemoveGame(Id);
            });

            Task.Factory.StartNew( () => {
                _EnergyThreadName = "GameEnergyThread." + Thread.CurrentThread.ManagedThreadId.ToString();
                while (true)
                {
                    if (_state == GameState.Playing)
                    {
                        ProcessEnergy();
                    }
                    else if (_state >= GameState.Recycling)
                    {
                        break;
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        private void ProcessEnergy()
        {
            foreach (Player player in Players.Values)
            {
                if (player.State == PlayerState.Playing)
                {
                    double inc = player.Cells.Count * 0.2;
                    player.AddEnergy(inc);
                }
            }
        }

        private void ProcessAttack(Command cmd, IMap map)
        {
            int time = 0;
            try
            {
                var cell = map.Locate(cmd.Target);
                var player = Core.Players.Get(cmd.Sender);
                
                if (cell != null && cell.CanAttack(player, ThreadNum))
                {
                    time = ActionLogic.Calc(cmd.Action, cell, player);
                    Task<bool>.Factory.StartNew(() =>
                    {
                        return cell.BeginChangeOwner(player, time, ThreadNum);
                    }).ContinueWith((task) =>
                    {
                        if (task.Result)
                            return cell.EndChangeOwner(player);
                        else
                            return false;
                    }).ContinueWith((task) => {
                        if (task.Result)
                            ProcessAttackResult(player, cell);

                        ClearLostCells();
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("cell pos:" + cmd.Target.ToString());
                Console.WriteLine("sender:" + cmd.Sender);
                Console.WriteLine("calced time:" + time.ToString());
                throw ex;
            }
        }

        private void ProcessVictory()
        {
            int iCount = 0;

            foreach (Player player in Players.Values)
            {
                if (player.State == PlayerState.Playing)
                {
                    iCount++;
                }
                if (iCount > 1)
                {
                    return;
                }
            }
            if (iCount == 1
                || (DateTime.Now - CreateTime).TotalHours > 1)
            {
                //win
                _state = GameState.Done;
            }

            

        }

        private Dictionary<int, Player> DefeatPlayers = new Dictionary<int, Player>();

        private void ProcessAttackResult(Player sender, Cell cell)
        {
            //if the attacked cell is a base, try to find all the player's bases.
            if (cell.Type == CellType.Base)
            {
                if (cell.LastOwner != null && cell.LastOwner != sender)
                {
                    var bases = cell.LastOwner.Bases;
                    //if the player just remain one base, which is the attacked one, the player is defeated.
                    if (bases.Count == 1)
                    {
                        //
                        cell.LastOwner.State = PlayerState.Defeat;
                        DefeatPlayers.Add(cell.LastOwner.Index, cell.LastOwner);
                        //release all the cells 
                        foreach (Cell c in cell.LastOwner.Cells.Values)
                        {
                            c.LastOwner = null;
                            c.OccupiedTime = DateTime.MinValue;
                            c.Owner = null;
                            c.State = CellState.Empty;
                            c.Type = CellType.Cell;
                        }
                    }

                    //change the type to normal cell if last owner is others
                    cell.Type = CellType.Cell;

                }

            }


        }

        private void ClearLostCells()
        {
            if (DefeatPlayers.Count == 0)
                return;

            foreach (Row row in _map.Rows)
            {
                foreach (Cell cell in row.Cells)
                {
                    if (DefeatPlayers.ContainsKey(cell.OwnerIndex))
                    {
                        cell.LastOwner = null;
                        cell.OccupiedTime = DateTime.MinValue;
                        cell.Owner = null;
                        cell.State = CellState.Empty;
                        cell.Type = CellType.Cell;
                    }
                }
            }
        }

        public void JoinGame(Player player)
        {
            player.State = PlayerState.Playing;
            this.Players.Add(player.Id, player);

            if (string.IsNullOrEmpty(this.Owner))
            {
                this.Owner = player.Id;
            }

            //alloc base
            while (true)
            {
                Random rnd = new Random(DateTime.Now.Millisecond);
                int y = rnd.Next(0, _map.Rows.Count - 1);
                int x = rnd.Next(0, _map.Rows[y].Count - 1);
                Cell cell = _map.Rows[y].Cells[x];
                lock (cell)
                {
                    if (cell.Type == CellType.Cell)
                    {
                        cell.Type = CellType.Base;
                        cell.State = CellState.Occupied;
                        cell.OccupiedTime = DateTime.MinValue;
                        cell.Owner = player;
                        player.Bases.Add(cell.Key, cell);
                        player.Cells.Add(cell.Key, cell);
                        break;
                    }
                    else
                        Thread.Sleep(100);
                }
            }
        }

        public bool Start()
        {
            if (this.Players.Count > 0)
            {
                _state = GameState.Playing;
                return true;
            }
            else
                return false;
        }

        public bool HasPlayer(string id)
        {
            if (this.Players.ContainsKey(id))
                return true;
            else
                return false;
        }

        public void Enqueue(Command cmd)
        {
            _commands.Enqueue(cmd);
        }
    }
}
