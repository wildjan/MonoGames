using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace BlackJack
{
    /// <summary>
    /// Provides a class for a deck of cards
    /// </summary>
    public class Deck
    {
        #region Fields

        List<Card> cards = new List<Card>();
        Texture2D cardTile, cardBack;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public Deck(Texture2D cardTileSprite, Texture2D cardBackSprite)
        {
            this.cardTile = cardTileSprite;
            this.cardBack = cardBackSprite;

            // fill the deck with cards
            foreach (Char suit in Constants.SUITS)
            {
                foreach (Char rank in Constants.RANKS)
                {
                    cards.Add(new Card(cardTile, cardBack, rank, suit));
                }
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether the deck is empty
        /// </summary>
        public bool Empty
        {
            get { return cards.Count == 0; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Cuts the deck of cards at the given location
        /// </summary>
        /// <param name="location">the location at which to cut the deck</param>
        public void Cut(int location)
        {
            int cutIndex = cards.Count - location;
            Card[] newCards = new Card[cards.Count];
            cards.CopyTo(cutIndex, newCards, 0, location);
            cards.CopyTo(0, newCards, location, cutIndex);
            cards.Clear();
            cards.InsertRange(0, newCards);

            //List<Card> newCards = new List<Card>(cards.Count);
            //for (int i = location; i < cards.Count; i++)
            //{
            //    newCards[i - location] = cards[i];
            //}
            //for (int i = 0; i < location; i++)
            //{
            //    newCards[i + location] = cards[i];
            //}
            //cards = newCards;
        }

        /// <summary>
        /// Shuffles the deck
        /// 
        /// Reference: http://download.oracle.com/javase/1.5.0/docs/api/java/util/Collections.html#shuffle%28java.util.List%29
        /// </summary>
        public void Shuffle()
        {
            Random rand = new Random();
            for (int i = cards.Count - 1; i > 0; i--)
            {
                int randomIndex = rand.Next(i + 1);
                Card tempCard = cards[i];
                cards[i] = cards[randomIndex];
                cards[randomIndex] = tempCard;
            }
        }

        /// <summary>
        /// Takes the top card from the deck. If the deck is empty, returns null
        /// </summary>
        /// <returns>the top card</returns>
        public Card TakeTopCard()
        {
            if (!Empty)
            {
                Card topCard = cards[cards.Count - 1];
                cards.RemoveAt(cards.Count - 1);
                return topCard;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Shows the top card from the deck. If the deck is empty, returns null
        /// </summary>
        /// <returns>the top card</returns>
        public Card ShowTopCard()
        {
            if (!Empty)
            {
                Card topCard = cards[cards.Count - 1];
                return topCard;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Returns the card to the deck
        /// </summary>
        /// <returns>the top card</returns>
        public void ReturnCard(Card card)
        {
            cards.Add(card);
        }

        #endregion
    }
}
