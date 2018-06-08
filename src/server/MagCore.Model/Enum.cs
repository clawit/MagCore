using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public enum CellState
    {
        Empty,
        Flicke,
        Occupied
    }

    public enum GameState
    {
        Wait,
        Playing,
        Done
    }
}
