using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Pong
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // a centre court support
        Texture2D background;
        Rectangle bckgrndRectangle = new Rectangle(0, 0,
            Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT);

        // objects
        Ball ball;
        Paddle padLeft, padRight;
        int padLeftVel, padRightVel;
        Random rand = new Random();

        // scoring
        int scoreLeft, scoreRight;
        string scoreLeftStr, scoreRightStr;

        // text display support
        SpriteFont font;
        Vector2 scoreLeftPos = new Vector2(
            Constants.WINDOW_WIDTH / 2 - Constants.SPAN, Constants.SPAN);
        Vector2 scoreRightPos = new Vector2(
            Constants.WINDOW_WIDTH / 2 + Constants.SPAN, Constants.SPAN);
        Vector2 fontOrigin;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // change resolution
            graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;
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

            // load background
            background = Content.Load<Texture2D>(@"graphics\background");

            // create the ball
             Spawn(true);

            // create paddles
            padLeft = new Paddle(Content, @"graphics\paddle", graphics.PreferredBackBufferHeight / 2, true);
            padRight = new Paddle(Content, @"graphics\paddle", graphics.PreferredBackBufferHeight / 2, false);

            // load font
            font = Content.Load<SpriteFont>(@"fonts\Arial20");

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

            KeyboardState key = Keyboard.GetState();

            // update the left paddle
            if (key.IsKeyDown(Keys.W)) padLeftVel = -1;
            else if (key.IsKeyDown(Keys.S)) padLeftVel = 1;
            else padLeftVel = 0;
            padLeft.Update(gameTime, padLeftVel);

            // update the right paddle
            if (key.IsKeyDown(Keys.Up)) padRightVel = -1;
            else if (key.IsKeyDown(Keys.Down)) padRightVel = 1;
            else padRightVel = 0;
            padRight.Update(gameTime, padRightVel);

            // manages collision (strokes)
            if (ball.CollisionRectangle.Intersects(padLeft.CollisionRectangle)
                || ball.CollisionRectangle.Intersects(padRight.CollisionRectangle))
            {
                ball.Stroke();
                ball.PlaySound();
                ball.Accelerate();
            }

            else
            {
                if (ball.LeftDrop)
                {
                    scoreRight++;
                    Spawn(false);
                }
                else if (ball.RightDrop)
                {
                    scoreLeft++;
                    Spawn(true);
                }
            }

            // update ball
            ball.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background, bckgrndRectangle, Color.White);
            ball.Draw(spriteBatch);
            padLeft.Draw(spriteBatch);
            padRight.Draw(spriteBatch);
            DrawTexts(spriteBatch);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /// <summary>
        /// Spawns the ball to the !left = right or to the left = left
        /// </summary>
        /// <<param name="left">direction</param>
        public void Spawn(bool left)
        {
            float velX = (float)rand.Next(10, 21) / 100;
            float velY = -(float)rand.Next(5, 11) / 100;
            if (left) velX *= -1;
            ball = new Ball(
                Content,
                @"graphics\ball",
                new Vector2(300, 200),
                new Vector2(velX, velY),
                @"audio\ballbounce");
        }

        /// <summary>
        /// Draws all texts
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        public void DrawTexts(SpriteBatch spriteBatch)
        {
            // Draw Score
            scoreLeftStr = scoreLeft.ToString();
            fontOrigin = font.MeasureString(scoreLeftStr) / 2;
            spriteBatch.DrawString(font, scoreLeftStr, scoreLeftPos, Color.White,
                0, fontOrigin, 1.4f, SpriteEffects.None, 0.5f);

            scoreRightStr = scoreRight.ToString();
            fontOrigin = font.MeasureString(scoreRightStr) / 2;
            spriteBatch.DrawString(font, scoreRightStr, scoreRightPos, Color.White,
                0, fontOrigin, 1.4f, SpriteEffects.None, 0.5f);
        }
    }
}
