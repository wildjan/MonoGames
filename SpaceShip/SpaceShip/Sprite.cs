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
    /// A class for a sprite
    /// </summary>
    class Sprite
    {
        #region Fields

        bool active = true;

        // drawing support
        Texture2D sprite;
        Point size, dimensions;
        Rectangle sourceRectangle;
        Vector2 origin;
        
        // velocity information
        Vector2 position;
        Vector2 velocity;
        float angle = 0;
        float angleVelocity;
        int radius;
        int lifeSpan;
        int age = 0;
        bool animated; 

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a sprite centered on the given x and y with the
        ///  given velocity and angle velocity and sprite image info
        /// </summary>
        /// <param name="sprite">the name of the sprite for the asteroid</param>
        /// <param name="position">the vector location of the center of the asteroid</param>
        /// <param name="velocity">the vector of velocity of the asteroid</param>
        /// <param name="angleVelocity">the angle velocity of the asteroid</param>
        /// <param name="info">sprite image info</param>
        public Sprite(Texture2D sprite,Vector2 position, Vector2 velocity, float angle, float angleVelocity, ImageInfo info)
        {
            // sets initial values
            this.sprite = sprite;
            this.position = position;
            this.velocity = velocity;
            this.angle = angle;
            this.angleVelocity = angleVelocity;
            origin = info.Origin;
            size = info.Size;
            dimensions = info.Dimensions;
            radius = info.Radius;
            lifeSpan = info.LifeSpan;
            animated = info.Animated;

            sourceRectangle = new Rectangle(0, 0, size.X, size.Y);
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
            // move the sprite
            position.X += velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            position.Y += velocity.Y * gameTime.ElapsedGameTime.Milliseconds;

            // if out of window, reveal the sprite on the oposite side
            if (position.X < 0) position.X = Constants.WINDOW_WIDTH;
            else position.X = position.X % Constants.WINDOW_WIDTH;
            if (position.Y < 0) position.Y = Constants.WINDOW_HEIGHT;
            else position.Y = position.Y % Constants.WINDOW_HEIGHT;

            // update angle
            angle += angleVelocity * gameTime.ElapsedGameTime.Milliseconds;

            // check for lifetime
            if (age < lifeSpan)
            {
                // sprite is older
                age++;
            }
             else
            {
                // deactivate sprite
                active = false;
            }
        }

        /// <summary>
        /// Draws the asteroid
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            if (animated)
            {
                sourceRectangle.X = age % dimensions.Y * size.X;
                sourceRectangle.Y = age / dimensions.Y * size.Y;
            }

            spriteBatch.Draw(sprite, position, sourceRectangle, Color.White, angle,
                            origin, 1.0f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// Determines if sprite colides with the otherSprite
        /// </summary>
        /// <param name="otherSprite">the other sprite</param>
        /// <returns>true if the sprite colides with the other sprite, else false</returns>
        public bool Colide(Sprite otherSprite)
        {
            return Game1.Distance(position, otherSprite.Position) < radius + otherSprite.Radius ;
        }

        /// <summary>
        /// Determines if sprite colides with the ship
        /// </summary>
        /// <param name="otherSprite">the other sprite</param>
        /// <returns>true if the sprite colides with the other sprite, else false</returns>
        public bool shipColide(Ship ship)
        {
            return Game1.Distance(position, ship.Position) < radius + ship.Radius;
        }

        #endregion

        #region Private methods



        #endregion
    }
}
