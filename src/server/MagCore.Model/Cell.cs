using MagCore.Model.Map;
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

        public bool BeginChangeOwner(Player sender, int time)
        {
            lock (this.Locker)
            {
                if (State == CellState.Flicke)
                    return false;
                else
                {
                    State = CellState.Flicke;
                    Owner = sender;
                    Thread.Sleep(time);

                    return true;
                }
            }
        }

        public void EndChangeOwner()
        {
            lock (this.Locker)
            {
                OccupiedTime = DateTime.Now;
                State = CellState.Occupied;
            }
        }

        public bool CanAttack(Player player)
        {
            if (this.State == CellState.Flicke
                || this.Type == CellType.Null)
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
