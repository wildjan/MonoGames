using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// Game constants
    /// </summary>
    public static class GameConstants
    {
        // resolution and the cell size
        public const int WindowWidth = 800;
        public const int WindowHeight = 600;
        public const int CellSize = 10;
        public const int NumRows = WindowHeight / CellSize;
        public const int NumCols = WindowWidth / CellSize;
        public const int BorderZone = 5;

        // movement support
        public static Vector2 UP = new Vector2(0, -1);
        public static Vector2 DOWN = new Vector2(0, 1);
        public static Vector2 LEFT = new Vector2(-1, 0);
        public static Vector2 RIGHT = new Vector2(1, 0);
        public static Vector2 STILL = new Vector2(0, 0);
        public const int DelayMilliseconds = 100;

    }
}
