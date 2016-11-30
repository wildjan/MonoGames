using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Pexeso
{
    /// <summary>
    /// A class for a playing card
    /// </summary>
    public class Card
    {
        // Cards tile loading support
        const int CARD_WIDTH = 140;
        const int CARD_HEIGHT = 140;
        const int TILE_ROWS = 2;
        const int TILE_COLS = 4;

        // Card drawing support
        const int BOARD_ROWS = 4;
        const int BOARD_COLS = 4;
        const int COL_OFFSET = 200;
        const int SPAN = 8;
        const int SMALL_OFFSETX = 95;
        const int SMALL_OFFSETY = 10;
        
        #region Fields

        // drawing support
        Texture2D cardsTile;
        Texture2D cardDownSide;
        Rectangle sourceCardRectangle;
        Rectangle sourceBackRectangle;
        Rectangle drawBigRectangle;
        Rectangle drawSmallRectangle;
        int position;
        int value;
        int spriteWidth;
        int spriteHeight;
        int windowWidth;
        int windowHeight;
        bool faceUp;
        bool revealed;
        bool magicEye;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a card with the given position and value
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="windowWidth">the window width</param>
        /// <param name="windowHeight">the window height</param>
        /// <param name="position">the position</param>
        /// <param name="value">the value</param>
        public Card(
            Texture2D cardsTile,
            Texture2D cardDownSide,
            int windowWidth, int windowHeight,
            int position, int value)
        {
            this.position = position;
            this.value = value;
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.cardsTile = cardsTile;
            this.cardDownSide = cardDownSide;

            // sets card dimension- HARD CODED (should be set with respect to the window dimensions 
            spriteWidth = CARD_WIDTH; //(TILE_ROWS * CARD_WIDTH + (TILE_ROWS + 1) * SPAN) / windowWidth * CARD_WIDTH;
            spriteHeight = CARD_HEIGHT; // (TILE_COLS * CARD_HEIGHT + (TILE_COLS + 1) * SPAN) / windowHeight * CARD_HEIGHT;
            
            // sets load and draw parameters
            faceUp = false;
            revealed = false;
            magicEye = false;
            sourceBackRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
            sourceCardRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
            SetSourceRectangleLocation(value);
            drawBigRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
            drawSmallRectangle = new Rectangle(0, 0, spriteWidth / 4, spriteHeight / 4);
            SetDrawRectangleLocation(position);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the card position
        /// </summary>
        public int Position
        {
            get { return position; }
        }
        
        /// <summary>
        /// Gets/Sets the card value
        /// </summary>
        public int Value
        {
            get { return value; }
            set 
            {
                this.value = value;
                SetSourceRectangleLocation(value);
            }
        }

        /// <summary>
        /// Gets/Sets whether or not the card is face up
        /// </summary>
        public bool FaceUp
        {
            get { return faceUp; }
            set { faceUp = value; }
        }

        /// <summary>
        /// Gets number of revealed cards
        /// </summary>
        public bool Revealed
        {
            get {return revealed; }
            set { revealed = value; }
        }

        /// <summary>
        /// Gets whether or not the magicEye is set
        /// </summary>
        public bool MagicEye
        {
            get { return magicEye; }
            set { magicEye = value; }
        }

        /// <summary>
        /// Gets drawRectangle
        /// </summary>
        public Rectangle DrawRectangle
        {
            get { return drawBigRectangle; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Flips the card over
        /// </summary>
        public void FlipOver()
        {
            faceUp = !faceUp;
        }

        /// <summary>
        /// Reverts Magic Eye
        /// </summary>
        public void TurnMagicEye()
        {
            magicEye = !magicEye;
        }

        /// <summary>
        /// Reveals a card
        /// </summary>
        public void Reveal()
        {
            revealed = true;
        }

        /// <summary>
        /// Draws a card
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // if a card is exposed
            if (FaceUp)
            {
                // draw an exposed card in the normal size
                spriteBatch.Draw(cardsTile, drawBigRectangle, sourceCardRectangle, Color.White);
            }

            // a card is not exposed
            else
            {
                // draw a back side of the card
                spriteBatch.Draw(cardDownSide, drawBigRectangle, sourceBackRectangle, Color.White);

                // if s magic feature is on
                if (magicEye)
                {
                    // draw a reduced card to the upper left corner of the back side of the card
                    spriteBatch.Draw(cardsTile, drawSmallRectangle, sourceCardRectangle, Color.White);
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the source rectangle location to correspond with the given frame
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        private void SetSourceRectangleLocation(int value)
        {
            // calculate X and Y based on frame number
            sourceCardRectangle.X = ((value - 1) % TILE_COLS) * CARD_WIDTH;
            sourceCardRectangle.Y = ((value - 1) / TILE_COLS) * CARD_HEIGHT;
        }
        /// <summary>
        /// Sets draw rectangles locations to correspond with the given card value
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        private void SetDrawRectangleLocation(int position)
        {
            // calculate X and Y based on frame number
            drawBigRectangle.X = COL_OFFSET + (position % BOARD_COLS) * spriteWidth + 
                (position % BOARD_COLS + 1) * SPAN;
            drawBigRectangle.Y = (position / BOARD_ROWS) * spriteHeight + (position / BOARD_COLS + 1) * SPAN;
            drawSmallRectangle.X = drawBigRectangle.X + SMALL_OFFSETX;
            drawSmallRectangle.Y = drawBigRectangle.Y + SMALL_OFFSETY;
        }

        #endregion
    }
}