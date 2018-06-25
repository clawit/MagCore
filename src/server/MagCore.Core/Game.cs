using MagCore.Model;
using MagCore.Model.Map;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
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

        private GameState _state;
        public GameState State { get { return _state; } }

        private ConcurrentQueue<Command> _commands = new ConcurrentQueue<Command>();

        internal Hashtable Players { get; } = new Hashtable();

        internal int ThreadId { get; set; }

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
                        string single = "{{\"Index\":{0}, \"Color\":{1}, \"Name\":\"{2}\"}}";
                        single = string.Format(single, player.Index, (int)player.Color, player.Name);
                        sPlayers.Add(single);
                    }
                    players = string.Join(",", sPlayers);
                }
                
            }

            return string.Format(json, Id, _map.Name, (int)State, players, _map.Cells());
        }

        public Game(IMap map)
        {
            _state = GameState.Wait;
            _map = map.Clone();     

            Task.Factory.StartNew(() => {
                ThreadId = Thread.CurrentThread.ManagedThreadId;
                while (_state != GameState.Recycling)
                {
                    if (_commands.IsEmpty)
                    {
                        Thread.Sleep(100);
                        continue;
                    }
                    else if (_commands.TryDequeue(out var cmd))
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
                        break;
                }
            }).ContinueWith((task) => {
                //Recycling


            });
        }

        private void ProcessAttack(Command cmd, IMap map)
        {
            var cell = map.Locate(cmd.Target);
            var player = Core.Players.Get(cmd.Sender);
            int time = ActionLogic.Calc(cmd.Action, cell, player);

            if (cell.CanAttack(player))
            {
                Task<bool>.Factory.StartNew(() =>
                {
                    return cell.BeginChangeOwner(player, time);
                }).ContinueWith((task) =>
                {
                    if (task.Result)
                        cell.EndChangeOwner();
                });
            }
        }

        public void JoinGame(Player player)
        {
            player.State = PlayerState.Playing;
            this.Players.Add(player.Id, player);

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
