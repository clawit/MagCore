using MagCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Core
{
    public static class ActionLogic
    {
        public static int Calc(Model.Action action, Cell target, Player sender)
        {
            if (target == null || sender == null)
            {
                return Int32.MinValue;
            }
            switch (action)
            {
                case Model.Action.Attack:
                    return _calcInternal(action, target, sender);
                default:
                    return 0;
            }

        }

        internal static int _calcInternal(Model.Action action, Cell target, Player sender)
        {
            if (target.State == CellState.Occupied)
            {
                int time = 1000;
                if (target.Owner != sender
                    && DateTime.Now > target.OccupiedTime)
                {
                    //attack other
                    var ts = DateTime.Now - target.OccupiedTime;
                    if (ts != null && ts.HasValue)
                    {
                        var duration = ts.Value.TotalSeconds;
                        if (duration > 60)
                            time = 3000;
                        else if (duration > 30)
                            time = 6000;
                        else if (duration > 15)
                            time = 9000;
                        else if (duration > 5)
                            time = 12000;
                        else
                            time = 15000;
                    }
                }

                //Console.WriteLine("Sleep time:" + time.ToString());
                return time;
            }
            else
                return 1000;
        }
    }
}
