using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace SpaceShip
{
    /// <summary>
    /// All the constants used in the game
    /// </summary>
    public static class Constants
    {
        // resolution
        public const int WINDOW_WIDTH = 800;
        public const int WINDOW_HEIGHT = 600;
        public const int DEBRIS_X = (WINDOW_WIDTH - WINDOW_WIDTH) / 2;
        public const int DEBRIS_Y = (WINDOW_HEIGHT - WINDOW_HEIGHT) / 2;

        // ship control
        public const float SHIP_SPEED = 0.01f;
        public const float FRICTION = 0.005f;
        public const float ANGLE_VEL = 0.003f;
        public static int SHIP_SAFE_DIST = 100;

        // the missile info
        public static Vector2 MISSILE_ORIGIN = new Vector2(10, 10);
        public static int MISSILE_RADIUS = 3;
        public const int MISSILE_LIFESPAN = 1;
        public const float MISSILE_SPEED = 0.01f;
        public const int MISSILE_COOLDOWN_MILLISECONDS = 500;

        // asteroid info
        public static Vector2 ASTEROID_ORIGIN = new Vector2(45, 45);
        public static int ASTEROID_RADIUS = 40;
        public static float ASTEROID_MIN_VEL = 0.01f;
        public static float ASTEROID_MAX_VEL = 0.1f;
        public static float ASTEROID_ANGLE_VEL = (float) Math.PI / 1000;
        public static int NUM_ASTEROIDS = 6;
        public const int TOTAL_DELAY_MILLISECONDS = 1000;

        // text info
        public static int OFFSET = 50;


    }

}
