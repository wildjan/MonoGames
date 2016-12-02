using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace BlackJack
{
    /// <summary>
    /// A class for a playing card
    /// </summary>
    public class Card
    {
        #region Fields

        Char rank;
        Char suit;
        bool faceUp;

        // drawing support
        Rectangle sourceCardRectangle;
        Rectangle sourceBackRectangle;
        Rectangle drawCardRectangle;
        Rectangle drawSmallRectangle;
        Texture2D cardBack, cardTile;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Constructs a card with the given rank and suit
        /// </summary>
        /// <param name="rank">the rank</param>
        /// <param name="suit">the suit</param>
        public Card(Texture2D cardTile, Texture2D cardBack, Char rank, Char suit)
        {
            this.rank = rank;
            this.suit = suit;
            this.cardTile = cardTile;
            this.cardBack = cardBack;

            faceUp = false;
            sourceBackRectangle = 
                new Rectangle(0, 0, Constants.CARD_SIZE.X, Constants.CARD_SIZE.Y);
            sourceCardRectangle = new Rectangle(0, 0,
                Constants.CARD_SIZE.X, Constants.CARD_SIZE.Y);
            SetSourceRectangleLocation(rank, suit);
            drawCardRectangle = new Rectangle(0, 0,
                Constants.CARD_SIZE.X, Constants.CARD_SIZE.Y);
            drawSmallRectangle = new Rectangle(0, 0,
                Constants.SMALL_CARD_SIZE.X, Constants.SMALL_CARD_SIZE.Y );
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the card rank
        /// </summary>
        public char Rank
        {
            get { return rank; }
        }

        /// <summary>
        /// Gets the card suit
        /// </summary>
        public char Suit
        {
            get { return suit; }
        }

        /// <summary>
        /// Gets whether or not the card is face up
        /// </summary>
        public bool FaceUp
        {
            get { return faceUp; }
        }

        /// <summary>
        /// Gets the Blackjack value for the card
        /// </summary>
        public int Value
        {
            get
            {
                return Constants.VALUES[rank];             
            }
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
        /// Draw the card
        /// </summary>
        /// <param name="spriteBatch">the sprite batch to use</param>
        /// <param name="position">the position</param>
        /// <param name="value">the value</param>

        public void Draw(SpriteBatch spriteBatch, Point position, bool magEye)
        {
            if (!magEye)
            {
                drawCardRectangle.X = position.X;
                drawCardRectangle.Y = position.Y;
                if (FaceUp)
                {
                    spriteBatch.Draw(cardTile, drawCardRectangle, sourceCardRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(cardBack, drawCardRectangle, sourceBackRectangle, Color.White);
                }
            }
            else
            {
                drawSmallRectangle.X = position.X;
                drawSmallRectangle.Y = position.Y;
                spriteBatch.Draw(cardTile, drawSmallRectangle, sourceCardRectangle, Color.White);
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Sets the source rectangle location to correspond with the given frame
        /// </summary>
        /// <param name="frameNumber">the frame number</param>
        private void SetSourceRectangleLocation(char rank, char suit)
        {
            // calculate X and Y based on frame number
            sourceCardRectangle.X = Array.IndexOf(Constants.RANKS, rank) * Constants.CARD_SIZE.X;
            sourceCardRectangle.Y = Array.IndexOf(Constants.SUITS, suit) * Constants.CARD_SIZE.Y;
        }

        #endregion
    }
}
