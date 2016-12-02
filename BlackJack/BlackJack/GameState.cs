using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlackJack
{
    /// <summary>
    /// An enumeration for the various game states
    /// </summary>
    public enum GameState
    {
        MenuIdle,
        Deal,
        Hit,
        Stand,
        MagEye,
        Quit
    }
}
