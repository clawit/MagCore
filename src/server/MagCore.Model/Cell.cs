using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MagCore.Model
{
    public class Cell
    {
        public Cell(int x, int y)
        {
            Position = new Position(x, y);
            State = CellState.Empty;
        }

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

        public bool BeginChangeOwner(int time)
        {
            lock (this.Locker)
            {
                if (State == CellState.Flicke)
                    return false;
                else
                {
                    State = CellState.Flicke;
                    Thread.Sleep(time);

                    return true;
                }
            }
        }

        public void EndChangeOwner(Player sender)
        {
            lock (this.Locker)
            {
                Owner = sender;
                OccupiedTime = DateTime.Now;
                State = CellState.Occupied;
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
