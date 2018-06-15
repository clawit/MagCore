using MagCore.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Core
{
    public static class ActionLogic
    {
        public static int Calc(Model.Action action, Cell target, Guid sender)
        {
            switch (action)
            {
                case Model.Action.Attack:
                    if (target.State == CellState.Empty)
                        return 1000;
                    else if (target.State == CellState.Occupied)
                    {
                        int time = 500;
                        if (target.Owner != sender 
                            && DateTime.Now > target.OccupiedTime)
                        {
                            var duration = (DateTime.Now - target.OccupiedTime).Value.TotalSeconds;
                            if (duration > 60)
                                time = 2000;
                            else if (duration > 30)
                                time = 5000;
                            else if (duration > 15)
                                time = 7000;
                            else if (duration > 5)
                                time = 15000;
                            else
                                time = 30000;
                        }
                        return time;
                    }
                    else
                        return 0;
                default:
                    return 0;
            }

        }
    }
}
