using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShip
{
    /// <summary>
    /// A class for a projectile
    /// </summary>
    public class Missile
    {
        #region Fields

        // existencionality
        bool active;
        int age = 0;

        // drawing support
        Texture2D sprite;
        Rectangle sourceRectangle;
        Vector2 origin = Constants.MISSILE_ORIGIN;

        // movement- position and velocity information
        Vector2 position, velocity;
        float angle;
        int radius = Constants.MISSILE_RADIUS;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a projectile with the given velocity
        /// </summary>
        /// <param name="type">the projectile type</param>
        /// <param name="sprite">the sprite for the projectile</param>
        /// <param name="position">the position of the center of the projectile</param>
        /// <param name="velocity">the velocity for the projectile</param>
        public Missile(Texture2D sprite, Vector2 position, Vector2 velocity, float angle)
        {
            this.sprite = sprite;
            this.position = position;
            this.velocity = velocity;
            this.angle = angle;
            active = true;
            sourceRectangle = new Rectangle(0, 0, sprite.Width / 2, sprite.Height);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets whether or not the projectile is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets the position of the missile
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the collision rectangle for the projectile
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the projectile's location and makes inactive when it
        /// leaves the game window
        /// </summary>
        public void Update(GameTime gameTime)
        {
            // move projectile
            position += velocity * gameTime.ElapsedGameTime.Milliseconds;

            // if out of window, reveal the projectile on the oposite side
            if (position.X < 0) position.X = Constants.WINDOW_WIDTH;
            else position.X = position.X % Constants.WINDOW_WIDTH;
            if (position.Y < 0) position.Y = Constants.WINDOW_HEIGHT;
            else position.Y = position.Y % Constants.WINDOW_HEIGHT;

            // check for lifetime
            age++;
            if (age > Constants.MISSILE_LIFESPAN)
            {
                // deactivate missile
                active = false;
            }
        }

        /// <summary>
        /// Draws the projectile
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, angle,
                            origin, 1.0f, SpriteEffects.None, 0f);
        }

        #endregion
    }
}