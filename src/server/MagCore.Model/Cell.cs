﻿using MagCore.Model.Map;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MagCore.Model
{
    public class Cell
    {
        public Cell(IMap map, int x, int y)
        {
            Position = new Position(x, y);
            State = CellState.Empty;
            _map = map;
        }

        private IMap _map = null;
        public Position Position { get; set; }

        public string Key => Position.ToString();

        public CellType Type { get; set; }

        public CellState State { get; set; }

        public int OwnerIndex {
            get {
                if (Owner == null || Owner.Id == Guid.Empty.ToString("N"))
                    return 0;
                else
                    return Owner.Index;
            }
        }

        public string OwnerId => Owner.Id;

        public Player Owner { get; set; }

        public DateTime? OccupiedTime { get; set; } = null;

        public object Locker = new object();

        public Player LastOwner { get; set; }
        public bool BeginChangeOwner(Player sender, int time, int thread)
        {
            lock (this.Locker)
            {
                if (State == CellState.Flicke
                    || sender.Energy < 1
                    || sender.ThreadLocker >= thread)
                    return false;
                else
                {
                    double inc = -0.1 * sender.Energy;
                    if (sender.Energy + inc <= 0)
                    {
                        return false;
                    }
                    sender.AddEnergy(inc);
                    
                    State = CellState.Flicke;
                    LastOwner = Owner;
                    Owner = sender;
                    if (!sender.Cells.ContainsKey(Key))
                        sender.Cells.Add(Key, this);

                    Interlocked.Increment(ref sender.ThreadLocker);
                    Thread.Sleep(time);
                    Interlocked.Decrement(ref sender.ThreadLocker);

                    return true;
                }
            }
        }

        public bool EndChangeOwner(Player sender)
        {
            lock (this.Locker)
            {
                if (sender.State == PlayerState.Defeat)
                {
                    OccupiedTime = DateTime.MinValue;
                    State = CellState.Empty;
                    LastOwner = null;
                    Owner = null;
                    return false;
                }
                else
                {
                    OccupiedTime = DateTime.Now;
                    State = CellState.Occupied;
                    if (LastOwner != null && LastOwner.Cells.ContainsKey(Key))
                        LastOwner.Cells.Remove(Key);
                    return true;
                }
                
            }
        }

        public bool CanAttack(Player player, int thread)
        {
            if (this.State == CellState.Flicke
                || this.Type == CellType.Null
                || player.State == PlayerState.Defeat
                || player.Energy < 1
                || player.ThreadLocker >= thread)
                return false;
            else
            {
                var siblings = _map.GetSiblings(this);
                if (siblings == null || siblings.Count <= 0)
                    return false;
                else
                {
                    foreach (Cell sibling in siblings)
                    {
                        if (sibling.Owner == player)
                            return true;
                    }

                    //else (nothing hitted in foreach)
                    return false;
                }
            }
        }

        public string ToJson()
        {
            string json = "{{ \"X\":{0}, \"Y\":{1}, \"Type\":{2}, \"State\":{3}, \"Owner\":{4} }}";
            json = string.Format(json, Position.X, Position.Y, (int)Type, (int)State, OwnerIndex);

            return json;
        }
    }
}
