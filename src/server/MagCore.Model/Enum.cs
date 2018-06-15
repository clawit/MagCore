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

    public enum CellType
    {
        NULL,   
        Empty,  
        Base,
        Stone,
    }

    public enum Action
    {
        Create,
        Join,
        Start,
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
        Red,
        Yellow,
        Green,
        Blue,
    }
}
