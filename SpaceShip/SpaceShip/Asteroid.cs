using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShip
{
    /// <summary>
    /// A class for an asteroid
    /// </summary>
    public class Asteroid
    {
        #region Fields

        bool active = true;

        // drawing support
        Texture2D sprite;
        Rectangle sourceRectangle;
        Vector2 origin = Constants.ASTEROID_ORIGIN;
        
        // velocity information
        Vector2 position;
        Vector2 velocity;
        float angle = 0;
        float angleVelocity;
        int radius = Constants.ASTEROID_RADIUS;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs an asteroid centered on the given x and y with the
        ///  given velocity and angle velocity
        /// </summary>
        /// <param name="sprite">the name of the sprite for the asteroid</param>
        /// <param name="position">the vector location of the center of the asteroid</param>
        /// <param name="velocity">the vector of velocity of the asteroid</param>
        /// <param name="angleVelocity">the angle velocity of the asteroid</param>
        public Asteroid(Texture2D sprite,Vector2 position, Vector2 velocity, float angleVelocity)
        {
            // sets initial values
            this.sprite = sprite;
            this.position = position;
            this.velocity = velocity;
            this.angleVelocity = angleVelocity;
            sourceRectangle = new Rectangle(0, 0, sprite.Width / 2, sprite.Height);
            origin = Constants.ASTEROID_ORIGIN;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets and sets whether or not the asteroid is active
        /// </summary>
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        /// <summary>
        /// Gets the position of the asteroid
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the radius od the asteroid
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the asteroid's position.
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            // move the asteroid
            position.X += velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            position.Y += velocity.Y * gameTime.ElapsedGameTime.Milliseconds;

            // if out of window, reveal the asteroid on the oposite side
            if (position.X < 0) position.X = Constants.WINDOW_WIDTH;
            else position.X = position.X % Constants.WINDOW_WIDTH;
            if (position.Y < 0) position.Y = Constants.WINDOW_HEIGHT;
            else position.Y = position.Y % Constants.WINDOW_HEIGHT;

            // update angle
            angle += angleVelocity * gameTime.ElapsedGameTime.Milliseconds;
         }

        /// <summary>
        /// Draws the asteroid
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, position, null, Color.White, angle,
                            origin, 1.0f, SpriteEffects.None, 0f);
        }

        #endregion

        #region Private methods


        #endregion
    }
}
