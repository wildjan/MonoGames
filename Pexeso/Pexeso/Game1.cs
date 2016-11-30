using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pexeso
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // instance variables cards
        Board board;

        // scoring support
        string scoreString = "Score: " + 0;

        // text display support
        SpriteFont font;
        Vector2 fontPos;

        // const resolution
        const int WINDOW_WIDTH = 800;
        const int WINDOW_HEIGHT = 600;

        // fields to keep track of game state
        static GameState state;
        bool gameRunning = true;

        // Cards tile and back saved so they don't have to
        // be loaded every time card is created
        Texture2D cardsTile, cardDownSide;


        // menu objects
        Menu mainMenu;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // change resolution
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
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
            // TODO: Add your initialization logic here

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

            // load font
            font = Content.Load<SpriteFont>(@"fonts\Arial20");

            // initialize menu objects
            mainMenu = new Menu(Content, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);

             // load card tile and back
            cardDownSide = Content.Load<Texture2D>(@"graphics\pexesoBack");
            cardsTile = Content.Load<Texture2D>(@"graphics\pexesoTile");

            // create board
            board = new Board(cardsTile, cardDownSide, graphics.PreferredBackBufferWidth,
                graphics.PreferredBackBufferHeight);
            fontPos = new Vector2(100f, 100f);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // process mouse based on game state
            MouseState mouse = Mouse.GetState();
            mainMenu.Update(mouse, gameRunning);

            // if the game is running
            if (gameRunning)
            {
                // and a magic eye feature is on
                if (state == GameState.MagEye)
                {
                    // show reduced cards as hint
                    board.TurnMagic();
                    state = GameState.Running;
                }

                // test end of game
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    Card card = board.CardClicked(mouse);
                    if (card != null)
                    {
                        gameRunning = !board.Update(card);
                    }
                }
            }

            // game is not running
            else
            {
                // and button 'New game' is pressed initialize a new game
                if (state == GameState.Play)
                {
                    board.Initialize();
                    gameRunning = true;
                    state = GameState.Running;
                }

                // and button Quit is pressed initialize a new game
                if (state == GameState.Quit)
                {
                    this.Exit();
                }
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

            spriteBatch.Begin();

            // draw board
            board.Draw(spriteBatch);

            // draw the main menu
            mainMenu.Draw(spriteBatch);

            // determine score string
            scoreString = "Score: " + board.NumTurns;

            // Find the center of the string
            Vector2 FontOrigin = font.MeasureString(scoreString) / 2;

            // Draw the string
            spriteBatch.DrawString(font, scoreString, fontPos, Color.DarkBlue,
                0, FontOrigin, 1.2f, SpriteEffects.None, 0.5f);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Changes the state of the game
        /// </summary>
        /// <param name="newState">the new game state</param>
        public static void ChangeState(GameState newState)
        {
            state = newState;
        }
    }
}
