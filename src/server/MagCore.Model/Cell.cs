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
            Owner = Guid.Empty;
        }

        public Position Position { get; set; }

        public string Key => Position.ToString();

        public CellType Type { get; set; }

        public CellState State { get; set; }

        public Guid Owner { get; set; }

        public DateTime? OccupiedTime { get; set; } = null;

        private object _locker { get; set; }

        public bool ChangeOwner(Guid sender, int time)
        {
            if (State == CellState.Flicke)
                return false;
            else
            {
                lock (this._locker)
                {
                    State = CellState.Flicke;
                    Thread.Sleep(time);
                    Owner = sender;
                    OccupiedTime = DateTime.Now;
                    State = CellState.Occupied;
                }

                return true;
            }
        }
    }
}
