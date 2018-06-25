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
                throw new ArgumentNullException();
            }
            switch (action)
            {
                case Model.Action.Attack:
                    if (target.State == CellState.Empty)
                        return 1000;
                    else if (target.State == CellState.Occupied)
                    {
                        int time = 1000;
                        if (target.Owner != sender 
                            && DateTime.Now > target.OccupiedTime)
                        {
                            //attack other
                            var duration = (DateTime.Now - target.OccupiedTime).Value.TotalSeconds;
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

                        //Console.WriteLine("Sleep time:" + time.ToString());
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
