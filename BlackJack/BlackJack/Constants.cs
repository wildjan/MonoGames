using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace BlackJack
{
    /// <summary>
    /// All the constants used in the game
    /// </summary>
    public static class Constants
    {
        // resolution
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;

        // Cards
        public static char[] SUITS = { 'C', 'S', 'H', 'D' };
        public static char[] RANKS = { 'A', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K' };
        public static Dictionary<char, int> VALUES = new Dictionary<char, int>()
       {
            { 'A', 1}, {'1', 1 }, {'2', 2 }, {'3', 3 }, {'4', 4 }, {'5', 5 },{'6', 6 },
            {'7', 7 }, {'8', 8 }, {'9', 9 },{'T', 10 },{'J', 10 }, {'Q', 10 }, {'K', 10 }
        };

        // card drawing
        public static readonly Point CARD_SIZE = new Point(73, 98);
        public static readonly Point SMALL_CARD_SIZE = new Point(48, 65);
        public static readonly Point SMALL_OFFSET = new Point(12, 16);
        public const int OFFSET = 250;
        public const int DEALER_Y = 240;
        public const int PLAYER_Y = 440;
        public const int SPAN = 5;
    }

 }
