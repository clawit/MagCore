using System;
using System.Collections.Generic;
using System.Text;

namespace MagCore.Model
{
    public enum CellState
    {
        Empty = 0,
        Flicke = 1,
        Occupied = 2,
    }

    public enum CellType
    {
        Null = 0,   
        Cell = 1,  
        Base = 2,
    }

    public enum Action
    {
        Attack
    }

    public enum GameState
    {
        Wait,
        Playing,
        Done,
        Recycling
    }

    public enum PlayerState
    {
        Leisure,
        Playing
    }

    public enum PlayerColor
    {
        Banana = 0,
        Cherry = 1,
        Grapes = 2,
        GreenMelon = 3,
        Lemon = 4,
        Mulberry = 5,
        Pear = 6,
        Pineapple = 7,
        Radish = 8,
        Watermelon = 9,
    }
}
