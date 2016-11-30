using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong
{
    /// <summary>
    /// A class for a paddle
    /// </summary>
    public class Paddle
    {
        #region Fields

        // graphic and drawing info
        Texture2D sprite;
        Rectangle drawRectangle;

        // game info
        int posX, posY;
        bool left;
        const int VELOCITY = 4;

        #endregion

        #region Constructors

        /// <summary>
        ///  Constructs a paddle
        /// </summary>
        /// <param name="sprite">the sprite</param>
        /// <param name="x">the x location of the center of the paddle</param>
        /// <param name="y">the y location of the center of the paddle</param>
        /// <param name="shootSound">the sound the paddle plays when shooting</param>
        public Paddle(ContentManager contentManager, string spriteName, int posY, bool left)
        {
            sprite = contentManager.Load<Texture2D>(spriteName);
            this.left = left;
            this.posY = posY;
            if (left)
            {
                posX = sprite.Width / 2;
            }
            else
            {
                posX = Constants.WINDOW_WIDTH - sprite.Width / 2;
            }

            drawRectangle = new Rectangle(
                posX - sprite.Width / 2,
                posY - sprite.Height / 2,
                sprite.Width,
                sprite.Height);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the collision rectangle for the paddle
        /// </summary>
        public Rectangle CollisionRectangle
        {
            get { return drawRectangle; }
        }

        #endregion

        #region Private properties

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the paddle's location based on mouse. Also fires 
        /// french fries as appropriate
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="key">the current state of the mouse</param>
        public void Update(GameTime gameTime, int velocity)
        {
            // paddle should only respond to input if it still on court
            posY += velocity * VELOCITY;
            drawRectangle.Y = posY - sprite.Height / 2;
            Clamp();
        }

        /// <summary>
        /// Draws the paddle
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Clamps the paddle
        /// </summary>
        private void Clamp()
        {
            // clamp to keep in the court
            if (drawRectangle.Y < 0)
            {
                drawRectangle.Y = 0;
            }
            else if (drawRectangle.Y > Constants.WINDOW_HEIGHT - drawRectangle.Height)
            {
                drawRectangle.Y = Constants.WINDOW_HEIGHT - drawRectangle.Height;
            }
        }

        #endregion
    }
}
