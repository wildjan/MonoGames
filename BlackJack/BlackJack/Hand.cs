using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackJack
{
    /// <summary>
    /// A class for a Blackjack hand
    /// </summary>
    public class Hand
    {
        #region Fields

        const int MAX_HAND_VALUE = 21;

        bool isDealer;
        List<Card> cards = new List<Card>();
        Point location;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ownerName">the name of the player owning the hand</param>
        public Hand(bool isDealer)
        {
            this.isDealer = isDealer;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the score for the hand
        /// </summary>
        public int Score
        {
            get
            {
                // add up score excluding Aces
                int numAces = 0;
                int score = 0;
                foreach (Card card in cards)
                {
                    if (card.Rank != 'A')
                    {
                        score += card.Value;
                    }
                    else
                    {
                        numAces++;
                    }
                }

                // if more than one ace, only one should ever be counted as 11
                if (numAces > 1)
                {
                    // make all but the first ace count as 1
                    score += numAces - 1;
                    numAces = 1;
                }

                // if there's an Ace,score it the best way possible
                if (numAces > 0)
                {
                    if (score + 11 <= MAX_HAND_VALUE)
                    {
                        // counting Ace as 11 doesn't bust
                        score += 11;
                    }
                    else
                    {
                        // count Ace as 1
                        score++;
                    }
                }

                return score;
            }
        }

        /// <summary>
        /// Gets the number of cards in hand
        /// </summary>
        public int NumberOfCards
        {
            get { return cards.Count; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a card to the hand
        /// </summary>
        /// <param name="card">the card to add</param>
        public void AddCard(Card card)
        {
            if (!isDealer || cards.Count != 0)
                card.FlipOver();
            cards.Add(card);
        }

        /// <summary>
        /// Takes a card from the hand and turns it if face up
        /// </summary>
        /// <returns"card">the card to return</returns>
        public Card Remove()
        {
            Card card = cards.First();
            cards.RemoveAt(0);
            if (card.FaceUp) card.FlipOver();
            return card;
        }

        /// <summary>
        /// Adds a card to the hand
        /// </summary>
        /// <param name="card">the card to add</param>
        public void FlipFirstCard()
        {
            if (isDealer || cards.Count != 0)
                cards[0].FlipOver();
        }

        /// <summary>
        /// Draws all the cards in the hand, if dealer the first card is hidden
        /// </summary>
        public void Draw(SpriteBatch spriteBatch, bool magEye, Card topCard)
        {
            foreach (Card card in cards)
            {
                int locY;
                if (isDealer) locY = Constants.DEALER_Y;
                else locY = Constants.PLAYER_Y;

                location = new Point(
                Constants.OFFSET + (Constants.CARD_SIZE.X + Constants.SPAN) * cards.IndexOf(card),
                locY);
                card.Draw(spriteBatch, location, false);
            }
            if (magEye)
            {
                int locY;
                if (isDealer)
                {
                    Card cardD = cards[0];
                    locY = Constants.DEALER_Y + Constants.SMALL_OFFSET.Y;
                    location = new Point(Constants.OFFSET + Constants.SMALL_OFFSET.X, locY);
                cardD.Draw(spriteBatch, location, true);
                }
                else
                {
                    locY = Constants.PLAYER_Y + Constants.SMALL_OFFSET.Y;
                    location = new Point(Constants.OFFSET + Constants.SMALL_OFFSET.X +
                        (Constants.CARD_SIZE.X + Constants.SPAN) * cards.Count, locY);
                topCard.Draw(spriteBatch, location, true);
                }
             }
        }

        #endregion

        #region Private methods

        #endregion
    }
}
