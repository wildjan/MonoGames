using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pexeso
{
    /// <summary>
    /// Provides a class for the PeXeSo board
    /// </summary>
    public class Board
    {
        // Support constants for the Board
        const int NUM_CARDS = 16;
        List<int> VALUES = new List<int>(new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 1, 2, 3, 4, 5, 6, 7, 8 });

        #region Fields
        int windowWidth;
        int windowHeight;
        Card card;
        List<Card> cardBoard = new List<Card>();
        List<Card> cardFlipped = new List<Card>();
        int numRevealed = 0;
        int numTurns;
        bool boardDone;
        Texture2D cardsTile, cardDownSide;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowWidth">the window width</param>
        /// <param name="windowHeight">the window height</param>
        public Board(Texture2D cardsTile,
                    Texture2D cardDownSide,
                    int windowWidth,
                    int windowHeight)
        {
            // fill the board with cards
            this.windowWidth = windowWidth;
            this.windowHeight = windowHeight;
            this.cardsTile = cardsTile;
            this.cardDownSide = cardDownSide;

            for (int position = 0; position < 16; position++)
            {
                int value = VALUES[position];
                card = new Card(
                    cardsTile,
                    cardDownSide,
                    windowWidth,
                    windowHeight, 
                    position, 
                    VALUES[position]);
                cardBoard.Add(card);
            }

            // Initialize game
            Initialize();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets number of revealed cards
        /// </summary>
        public int NumRevealed
        {
            get {
                numRevealed = 0;
                foreach (Card card in cardBoard)
                {
                    if (card.Revealed)
                    {
                        numRevealed++;
                    }

                }
                return numRevealed; }
        }

        /// <summary>
        /// Gets number of turns
        /// </summary>
        public int NumTurns
        {
            get {return numTurns; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Initializes game
        /// </summary>
        public void Initialize()
        {
            // fill the board with cards
            numTurns = 0;
            //numRevealed = 0;
            boardDone = false;
            cardFlipped.Clear();
            Shuffle();
            Shuffle();
            for (int position = 0; position < 16; position++)
            {
                int value = VALUES[position];
                card = cardBoard[position];
                card.Value = value;
                card.FaceUp = false;
                card.MagicEye = false;
                card.Revealed = false;
            }
            Console.WriteLine(NumRevealed);
        }

        /// <summary>
        /// Updates Board
        /// </summary>
        /// <returns>
        /// Returns status of game
        /// </returns>
        public bool Update(Card card)
        {
            // Manage card turning and game logic
            if (!card.FaceUp)
            {
                // How many cards are flipped
                if (cardFlipped.Count == 2)
                {
                    //  two card flipped
                    if (cardFlipped[0].Value != cardFlipped[1].Value)
                    {
                        // cards are not the same
                        HideFlipped();
                    }
                    else
                    {
                        cardFlipped[0].Reveal();
                        cardFlipped[1].Reveal();
                    }
                    cardFlipped.Clear();
                }
                else if (cardFlipped.Count == 1)
                {
                    // one card flipped increase score
                    numTurns++;
                }
                // flip the clicked card
                card.FlipOver();
                cardFlipped.Add(card);
            }
            if (NumRevealed + cardFlipped.Count == NUM_CARDS)
            {
                Console.WriteLine(NumRevealed + cardFlipped.Count());
                cardFlipped.Clear();
                boardDone = true;
            }
            return boardDone;

        }

        /// <summary>
        /// Draw cards and buttons
        /// </summary>
        /// <param name="location">the location at which to cut the deck</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Card card in cardBoard)
            {
                card.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Applies magic eye feature
        /// </summary>
        public void TurnMagic()
        {
            foreach (Card card in cardBoard)
            {
                card.TurnMagicEye();
            }
        }

        /// <summary>
        /// Determines value of the card clicked
        /// </summary>
        /// <return>Clicked card</return> 
        public Card CardClicked(MouseState mouse)
        {
            Card retCard = null;
            foreach (Card card in cardBoard)
            {
                if (card.DrawRectangle.Contains(mouse.X, mouse.Y))
                { retCard = card; }
            }
            return retCard;
        }

        #endregion

        #region private methods

        /// <summary>
        /// Shuffles the deck
        /// </summary>
        private void Shuffle()
        {
            Random rand = new Random();
            for (int i = VALUES.Count - 1; i > 0; i--)
            {
                int randomIndex = rand.Next(i + 1);
                int tempValue = VALUES[i];
                VALUES[i] = VALUES[randomIndex];
                VALUES[randomIndex] = tempValue;
            }
        }
        
        /// <summary>
        /// Hides flipped cards (they are not the same]
        /// </summary>
        private void HideFlipped()
        {
            foreach (Card tmpCard in cardFlipped)
            {
                tmpCard.FlipOver();
            }
        }
        
        #endregion
    }
}
