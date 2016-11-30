using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pong
{
    /// <summary>
    /// A class for a ball
    /// </summary>
    public class Ball
    {
        #region Fields

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle;

        // movement information
        Vector2 position;
        Vector2 velocity;

        // sound effects
        SoundEffect bounceSound;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a ball centered on the given x and y with the
        ///  given velocity
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="spriteName">the name of the sprite for the ball</param>
        /// <param name="position">the position of the center of the ball</param>
        /// <param name="velocity">vector2D of velocity of the ball</param>
        /// <param name="soundName">the name of the bouncing sound</param>
        public Ball(ContentManager contentManager, string spriteName, Vector2 position,
            Vector2 velocity, string soundName)
        {
            this.position = position;
            this.velocity = velocity;
            LoadContent(contentManager, spriteName, position, soundName);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the ball
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        /// <summary>
        /// Gets the left drop
        /// </summary> 
        public bool LeftDrop
        {
            get
            {
                if (drawRectangle.X < 0)
                    return true;
                else return false;
            }      
        }

        /// <summary>
        /// Gets the right drop
        /// </summary> 
        public bool RightDrop
        {
            get
            {
                if ((drawRectangle.X + drawRectangle.Width) > Constants.WINDOW_WIDTH)
                    return true;
                else return false;
            }
        }
  
        #endregion

        #region Public methods

        /// <summary>
        /// Updates the ball's location, bouncing if necessary. Also has
        /// the ball fire a projectile when it's time to
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            // move the ball
            position += velocity * gameTime.ElapsedGameTime.Milliseconds;
            drawRectangle.X = (int)(position.X - sprite.Width / 2);
            drawRectangle.Y = (int)(position.Y - sprite.Height / 2);

            // bounce as necessary
            BounceTopBottom();
        }

        /// <summary>
        /// Draws the ball
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        /// <summary>
        /// Bounces the ball off the left or right paddle if hit
        /// </summary>
        public void Stroke()
        {
            velocity.X *= -1;
            Accelerate();
        }

        /// <summary>
        /// accelerate ball
        /// </summary>
        public void Accelerate()
        {
            velocity *= 1.05f;
        }

        public void PlaySound()
        {
            this.bounceSound.Play();
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the teddy bear
        /// </summary>
        /// <param name="contentManager">the content manager to use</param>
        /// <param name="spriteName">the name of the sprite for the ball</param>
        /// <param name="x">the x location of the center of the ball</param>
        /// <param name="y">the y location of the center of the ball</param>
        private void LoadContent(ContentManager contentManager, string spriteName,
            Vector2 position, string soundName)
        {
            // load content and set remainder of draw rectangle
            sprite = contentManager.Load<Texture2D>(spriteName);
            drawRectangle = new Rectangle(
                (int)(position.X - sprite.Width / 2),
                (int)(position.Y - sprite.Height / 2),
                sprite.Width,
                sprite.Height);
            bounceSound = contentManager.Load<SoundEffect>(soundName);
        }

        /// <summary>
        /// Bounces the ball off the top and bottom window borders if necessary
        /// </summary>
        private void BounceTopBottom()
        {
            if (drawRectangle.Y < 0)
            {
                // bounce off top
                drawRectangle.Y = 0;
                velocity.Y *= -1;
            }
            else if ((drawRectangle.Y + drawRectangle.Height) > Constants.WINDOW_HEIGHT)
            {
                // bounce off bottom
                drawRectangle.Y = Constants.WINDOW_HEIGHT - drawRectangle.Height;
                velocity.Y *= -1;
            }
        }

        #endregion
    }
}
