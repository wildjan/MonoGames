using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GuessTheNumber
{
    /// <remarks>
    /// A number tile
    /// </remarks>
    public class NumberTile
    {
        #region Fields

        // original length of each side of the tile
        int originalSideLength;

        // whether or not this tile is the correct number
        bool isCorrectNumber;

        // drawing support
        Texture2D normalTexture;
        Rectangle drawRectangle;
        Rectangle sourceRectangle;

        // Increment 5: field for blinking tile texture
        Texture2D blinkingTexture;

        // Increment 5: field for current texture
        Texture2D currentTexture;

        // blinking support
        const int TotalBlinkMilliseconds = 4000;
        Timer blinkingTimer = new Timer(TotalBlinkMilliseconds);
        const int FrameBlinkMilliseconds = 1000;
        Timer frameBlinkingTimer = new Timer(FrameBlinkMilliseconds);

        // Increment 4: fields for shrinking support
        const int TotalShrinkMilliseconds = 1000;
        Timer shrinkingTimer = new Timer(TotalShrinkMilliseconds);

        // Increment 4: fields to keep track of visible, blinking, and shrinking
        bool isBlinking = false;
        bool isShrinking = false;
        bool isVislible = true;

        // Increment 4: fields for click support
        bool clickStarted = false;
        bool buttonReleased = true;

        // Increment 5: sound effect field
        SoundEffect soundEffect;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="center">the center of the tile</param>
        /// <param name="sideLength">the side length for the tile</param>
        /// <param name="number">the number for the tile</param>
        /// <param name="correctNumber">the correct number</param>
        public NumberTile(ContentManager contentManager, Vector2 center, int sideLength,
            int number, int correctNumber)
        {
            // set original side length field
            this.originalSideLength = sideLength;

            // load content for the tile and create draw rectangle
            LoadContent(contentManager, number);
            drawRectangle = new Rectangle((int)center.X - sideLength / 2,
                 (int)center.Y - sideLength / 2, sideLength, sideLength);

            // start timers
            blinkingTimer.Start();
            shrinkingTimer.Start();
            frameBlinkingTimer.Start();

            // set isCorrectNumber flag
            isCorrectNumber = number == correctNumber;

            // Increment 5: load sound effect field to correct or incorrect sound effect
            // based on whether or not this tile is the correct number
            if (isCorrectNumber) soundEffect = contentManager.Load<SoundEffect>(@"audio\explosion");
            else soundEffect = contentManager.Load<SoundEffect>(@"audio\loser");
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the tile based on game time and mouse state
        /// </summary>
        /// <param name="gameTime">the current GameTime</param>
        /// <param name="mouse">the current mouse state</param>
        /// <return>true if the correct number was guessed, false otherwise</return>
        public bool Update(GameTime gameTime, MouseState mouse)
        {
            // Increments 4 and 5: add code for shrinking and blinking support
            if (isBlinking)
            {
                blinkingTimer.Update(gameTime);

                // test if overall blinking is over
                if (!blinkingTimer.IsRunning)
                {
                    // game is over
                    return true;
                }
                else
                {
                    frameBlinkingTimer.Update(gameTime);

                    // test if blinking frame is over
                    if (!frameBlinkingTimer.IsRunning)
                    {
                        // move the source rectangle left or right
                        if (sourceRectangle.X == 0)
                        {
                            sourceRectangle.X = normalTexture.Width / 2;
                        }
                        else
                        {
                            sourceRectangle.X = 0;
                        }

                        frameBlinkingTimer.Start();
                    }
                }
            }

            else if (isShrinking)
            {
                shrinkingTimer.Update(gameTime);
                float ratio = 1 - (float) shrinkingTimer.ElapsedMilliseconds / TotalShrinkMilliseconds;
                int sideLenght = (int)(originalSideLength * ratio);

                // check if shrinking is over
                if (sideLenght > 0)
                {
                    drawRectangle.Width = sideLenght;
                    drawRectangle.Height = sideLenght;
                }
                else
                {
                    isVislible = false;
                }
            }

            else
            {
                // Increment 4: add code to highlight/unhighlight the tile
                if (drawRectangle.Contains(mouse.X, mouse.Y))
                {
                    sourceRectangle.X = normalTexture.Width / 2;

                    // click processing
                    // check for click started on button
                    if (mouse.LeftButton == ButtonState.Pressed &&
                        buttonReleased)
                    {
                        clickStarted = true;
                        buttonReleased = false;
                    }
                    else if (mouse.LeftButton == ButtonState.Released)
                    {
                        buttonReleased = true;

                        // if click finished on button, change game state
                        if (clickStarted)
                        {
                            if (isCorrectNumber)
                            {
                                isBlinking = true;
                                currentTexture = blinkingTexture;
                                sourceRectangle.X = 0;
                            }
                            else
                            {
                                isShrinking = true;
                            }

                            // Increment 5: play sound effect
                            soundEffect.Play();
                            clickStarted = false;
                        }
                    }
                }
                else
                {
                    sourceRectangle.X = 0;
                }
            }

            // if we get here, return false
            return false;
        }

        /// <summary>
        /// Draws the number tile
        /// </summary>
        /// <param name="spriteBatch">the SpriteBatch to use for the drawing</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // Increments 3, 4, and 5: draw the tile
            if (isVislible)
            {
                spriteBatch.Draw(currentTexture, drawRectangle, sourceRectangle, Color.White);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Loads the content for the tile
        /// </summary>
        /// <param name="contentManager">the content manager</param>
        /// <param name="number">the tile number</param>
        private void LoadContent(ContentManager contentManager, int number)
        {
            // convert the number to a string
            string numberString = ConvertIntToString(number);

            // Increment 3: load content for the tile and set source rectangle
            normalTexture = contentManager.Load<Texture2D>(@"graphics\" + numberString);
            sourceRectangle = new Rectangle(0, 0, normalTexture.Width / 2, normalTexture.Height);

            // Increment 5: load blinking tile texture
            blinkingTexture = contentManager.Load<Texture2D>(@"graphics\blinking" + numberString);

            // Increment 5: set current texture
            currentTexture = normalTexture;
        }

        /// <summary>
        /// Converts an integer to a string for the corresponding number
        /// </summary>
        /// <param name="number">the integer to convert</param>
        /// <returns>the string for the corresponding number</returns>
        private String ConvertIntToString(int number)
        {
            switch (number)
            {
                case 1:
                    return "one";
                case 2:
                    return "two";
                case 3:
                    return "three";
                case 4:
                    return "four";
                case 5:
                    return "five";
                case 6:
                    return "six";
                case 7:
                    return "seven";
                case 8:
                    return "eight";
                case 9:
                    return "nine";
                default:
                    throw new Exception("Unsupported number for number tile");
            }

        }

        #endregion
    }
}
