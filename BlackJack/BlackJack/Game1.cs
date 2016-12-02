using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace BlackJack
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // scoring, status and outcome support
        int score = 0;
        string scoreString = "Score: " + 0;
        string outcome = "You went bust and lose!";
        string status = "Hit or stand?";
        string dhand = "DD";
        string phand = "PP";

        // text display support
        SpriteFont font;
        Vector2 scorePos = new Vector2(680, 80);
        Vector2 outcomePos = new Vector2(560, 205);
        Vector2 statusPos = new Vector2(480, 400);
        Vector2 dhandPos = new Vector2(220, 290);
        Vector2 phandPos = new Vector2(220, 490);
        Vector2 fontOrigin;

        // status of the game support
        static GameState state;
        public bool magEye = false;
        bool inPlay = false;

        // menu
        Menu menu;

        // game objects
        static Texture2D boardSprite, cardTileSprite, cardBackSprite;
        Rectangle boardRectangle;
        Deck deck;
        Hand dealerHand, playerHand;

        // initialize random for cut
        Random rand = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // change screen resolution and mouse visibility
            graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // DONE: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // load a game content here
            cardTileSprite = Content.Load<Texture2D>(@"graphics\cards");
            cardBackSprite = Content.Load<Texture2D>(@"graphics\cardb");
            boardSprite = Content.Load<Texture2D>(@"graphics\BlackJackBoard");
            boardRectangle = new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

            // load font
            font = Content.Load<SpriteFont>(@"fonts\Arial20");

            // initialize menu objects
            menu = new Menu(Content, graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);

            // initialize new game
            deck = new Deck(cardTileSprite, cardBackSprite);
            dealerHand = new Hand(true);
            playerHand = new Hand(false);
            Deal();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // DONE: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // process mouse based on game state
            MouseState mouse = Mouse.GetState();
            menu.Update(mouse);

            // Game termination
            if (state == GameState.Quit)
            {
                this.Exit();
            }

            // Magic Eye- debugging state
            if (state == GameState.MagEye)
            {
                magEye = !magEye;
                state = GameState.MenuIdle;
            }

            // Deal
            if (state == GameState.Deal)
            {
                Deal();
                state = GameState.MenuIdle;
            }
            // Hit
            if (state == GameState.Hit)
            {
                Hit();
                state = GameState.MenuIdle;
            }

            // Stand
            if (state == GameState.Stand)
            {
                Stand();
                state = GameState.MenuIdle;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // DONE: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(boardSprite, boardRectangle, Color.White);
            Card topCard = deck.ShowTopCard();
            dealerHand.Draw(spriteBatch, magEye, topCard);
            playerHand.Draw(spriteBatch, magEye, topCard);
            menu.Draw(spriteBatch);
            DrawTexts(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Changes the state of the game,
        /// </summary>
        /// <param name="newState">the new game state</param>
        public static void ChangeState(GameState newState)
        {
            state = newState;
        }

        /// <summary>
        /// Deals cards to both hands
        /// </sumary>
        public void Deal()
        {
            // player gave up current game
            if (inPlay)
            {
                score--;
            }

            // return cards from hands to the deck
            while (dealerHand.NumberOfCards > 0)
            {
                Card card = dealerHand.Remove();
                deck.ReturnCard(card);
            }
            while (playerHand.NumberOfCards > 0)
            {
                Card card = playerHand.Remove();
                deck.ReturnCard(card);
            }

            // Prepares the deck for the game and deals two cards to each player
            deck.Shuffle();
            int cutNumber = rand.Next(1, 50);
            deck.Cut(cutNumber);
            playerHand.AddCard(deck.TakeTopCard());
            dealerHand.AddCard(deck.TakeTopCard());
            playerHand.AddCard(deck.TakeTopCard());
            dealerHand.AddCard(deck.TakeTopCard());
            inPlay = true;
            outcome = "Game is in progress...";
            status = "Hit or stand?";
        }

        /// <summary>
        /// Hits- deal another card to the player
        /// </summary>
        public void Hit()
        {
            if (inPlay)
            {
                playerHand.AddCard(deck.TakeTopCard());
                if (playerHand.Score > 21)
                {
                    inPlay = false;
                    score--;
                    outcome = "You went bust and lose!";
                    status = "New deal?";
                }
            }
        }

        /// <summary>
        /// Hits- deal another card to the player and process hand
        /// </summary>
        public void Stand()
        {
            // if player chose stand it is dealer's turn
            if (inPlay)
            {
                inPlay = false;
                magEye = false;

                // flip the first card
                dealerHand.FlipFirstCard();

                // while dealer's hand score is lower than 17 (rule?)
                while (dealerHand.Score < 17)
                {
                    // add him a card
                    dealerHand.AddCard(deck.TakeTopCard());
                }
                // is dealer busted?
                if (dealerHand.Score > 21)
                {
                    score += 1;
                    outcome = "Dealer went bust you won!";
                }
                // and winner is ...
                else
                {
                    if (playerHand.Score > dealerHand.Score)
                    {
                        // ... player
                        score++;
                        outcome = "You won!";
                    }
                    else
                    {
                        // ... dealer
                        score--;
                        outcome = "You lose!";
                    }
                }
                status = "New deal?";
            }
        }

        /// <summary>
        /// Draws all texts
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        public void DrawTexts(SpriteBatch spriteBatch)
        {
            // Draw Score
            scoreString = "Score: " + score.ToString();
            fontOrigin = font.MeasureString(scoreString) / 2;
            spriteBatch.DrawString(font, scoreString, scorePos, Color.DarkRed,
                0, fontOrigin, 1.4f, SpriteEffects.None, 0.5f);

            // Draw outcome
            fontOrigin = font.MeasureString(outcome) / 2;
            spriteBatch.DrawString(font, outcome, outcomePos, Color.Purple,
                0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            // Draw status
            fontOrigin = font.MeasureString(status) / 2;
            spriteBatch.DrawString(font, status, statusPos, Color.LightGreen,
                0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
            if (magEye)
            {
                // Draw the dealers hand value
                dhand = dealerHand.Score.ToString();
                fontOrigin = font.MeasureString(dhand) / 2;
                spriteBatch.DrawString(font, dhand, dhandPos, Color.LightGray,
                    0, fontOrigin, 0.8f, SpriteEffects.None, 0.5f);

                // Draw the players hand value
                phand = playerHand.Score.ToString();
                fontOrigin = font.MeasureString(phand) / 2;
                spriteBatch.DrawString(font, phand, phandPos, Color.LightGray,
                    0, fontOrigin, 0.8f, SpriteEffects.None, 0.5f);
            }
        }
    }
}
