using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SpaceShip
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // background drawing support
        Texture2D nebula, debris, splash;
        Rectangle nebulaRectangle = new Rectangle(0, 0,
                Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT);
        Rectangle debrisRectangle1 = new Rectangle(0, Constants.DEBRIS_Y,
                Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT);
        Rectangle debrisRectangle2 = new Rectangle(0, Constants.DEBRIS_Y,
                Constants.WINDOW_WIDTH, Constants.WINDOW_HEIGHT);
        Rectangle splashRectangle = new Rectangle(200, 150, 400, 300);
        int wtime;

        // sprites
        Texture2D shipSprite;
        static Texture2D missileSprite;
        Texture2D asteroidSprite0, asteroidSprite1, asteroidSprite2, asteroidSprite3;
        Texture2D[] ASTEROID_SPRITES;
        Texture2D explosion0, explosion1, explosion2, explosion3;
        Texture2D[] EXPLOSION_TILES;

        // objects
        static Ship ship;
        static List<Sprite> missileGroup = new List<Sprite>();
        static List<Sprite> asteroidGroup = new List<Sprite>();
        static List<Sprite> explosionGroup = new List<Sprite>();

        // object's image info
        ImageInfo shipInfo, asteroidInfo, explosionInfo;
        static ImageInfo missileInfo;

        // the background music support
        SoundEffectInstance soundtrack;
        SoundEffect soundtrackEffect;

        // the ship thrust sound support
        SoundEffectInstance thrustSound;
        SoundEffect thrustEffect;

        // the other sounds support
        SoundEffect missileSound, explosionSound;

        // delay support for asteroid spawning
        int elapsedDelayMilliseconds = 0;

        // game status support
        int score = 0;
        int lives = 3;
        bool death = false;
        bool started = false;

        // drawing text support
        string livesStr = "Lives: " + 3;
        string scoreStr = "Score: " + 0;
        SpriteFont font;
        Vector2 scorePos = new Vector2(
             Constants.WINDOW_WIDTH - Constants.OFFSET, Constants.OFFSET);
        Vector2 livesPos = new Vector2(
            Constants.OFFSET, Constants.OFFSET);
        Vector2 fontOrigin;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // change ressolution
            graphics.PreferredBackBufferWidth = Constants.WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = Constants.WINDOW_HEIGHT;

            // set mouse visibility
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

            // initialize the random generator
            RandomNumberGenerator.Initialize();

            // Initialize sprite image info
            shipInfo = new ImageInfo(new Vector2(45, 45),
                new Point(90, 90), new Point(1, 2), 35);
            missileInfo = new ImageInfo(new Vector2(10, 10),
                new Point(20, 20), new Point(1, 1), 3, 70, false);
            asteroidInfo = new ImageInfo(new Vector2(45, 45),
                new Point(90, 90), new Point(1, 1), 40);
            explosionInfo = new ImageInfo(new Vector2(64, 64),
                new Point(128, 128), new Point(2, 12), 64, 24, true);

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

            // DONE: use this.Content to load your game content here

            nebula = Content.Load<Texture2D>(@"graphics\nebula");
            debris = Content.Load<Texture2D>(@"graphics\debris2");
            splash = Content.Load<Texture2D>(@"graphics\splash");

            // load background music
            soundtrackEffect = Content.Load<SoundEffect>(@"audio\soundtrack");
            soundtrack = soundtrackEffect.CreateInstance();
            soundtrack.IsLooped = true;
            soundtrack.Play();
            soundtrack.Pause();

            // load audio content for the thrust sound
            thrustEffect = Content.Load<SoundEffect>(@"audio\thrust");
            thrustSound = thrustEffect.CreateInstance();
            thrustSound.IsLooped = true;

            // load the other sounds
            missileSound = Content.Load<SoundEffect>(@"audio\missile");
            explosionSound = Content.Load<SoundEffect>(@"audio\explosion");

            // define sprites and objects
            missileSprite = Content.Load<Texture2D>(@"graphics\shot3");
            shipSprite = Content.Load<Texture2D>(@"graphics\double_ship");
            ship = new Ship(shipSprite, thrustSound, missileSound, shipInfo,
                            new Vector2(
                                graphics.PreferredBackBufferWidth / 2,
                                graphics.PreferredBackBufferHeight / 2));

            asteroidSprite0 = Content.Load<Texture2D>(@"graphics\asteroid_blend");
            asteroidSprite1 = Content.Load<Texture2D>(@"graphics\asteroid_blue");
            asteroidSprite2 = Content.Load<Texture2D>(@"graphics\asteroid_brown");
            asteroidSprite3 = Content.Load<Texture2D>(@"graphics\drTRock");
            ASTEROID_SPRITES = new Texture2D[] { asteroidSprite0, asteroidSprite1, asteroidSprite2, asteroidSprite3 };

            explosion0 = Content.Load<Texture2D>(@"graphics\explosion_alpha");
            explosion1 = Content.Load<Texture2D>(@"graphics\explosion_blue");
            explosion2 = Content.Load<Texture2D>(@"graphics\explosion_blue2");
            explosion3 = Content.Load<Texture2D>(@"graphics\explosion_orange");
            EXPLOSION_TILES = new Texture2D[] { explosion0, explosion1, explosion2, explosion3 };

            // load font
            font = Content.Load<SpriteFont>(@"fonts\Arial20");
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

            // DONE: Add your update logic here

            // Update background
            wtime += 1;
            wtime %= Constants.WINDOW_WIDTH;
            debrisRectangle1.X = wtime;
            debrisRectangle2.X = wtime - Constants.WINDOW_WIDTH;

             // if started update game objects
            if (started)
            {
                // gets the keyboard state
                KeyboardState key = Keyboard.GetState();

                // update timer and add  asteroids
                elapsedDelayMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedDelayMilliseconds >= Constants.TOTAL_DELAY_MILLISECONDS
                    && asteroidGroup.Count < Constants.NUM_ASTEROIDS)
                {
                    // timer expired and number of asteroids is low, so spawn new asteroid
                    SpawnAsteroid(ship.Position);

                    // restart timer
                    elapsedDelayMilliseconds = 0;
                }

                // update the ship
                ship.Update(gameTime, key);

                // update all sprite groups (missiles, asteroids, explosions)
                ProcessSpriteGroup(gameTime, missileGroup);
                ProcessSpriteGroup(gameTime, asteroidGroup);
                ProcessSpriteGroup(gameTime, explosionGroup);

                // manage missiles collisions with asteroids
                score += 10 * GroupGroupColide(missileGroup, asteroidGroup, explosionSound);

                // manage ship collisions with asteroids
                death = ShipColide(asteroidGroup, explosionSound);
                if (death)
                {
                    // the ship collided with an asteroid
                    lives--;
                    if (lives <= 0)
                    {
                        // the ship is definittely dead - game is over
                        // clear all groups, create a new ship and invoke splash
                        // pause soundtrack
                        asteroidGroup.Clear();
                        missileGroup.Clear();
                        explosionGroup.Clear();
                        soundtrack.Pause();
                        ship = new Ship(shipSprite, thrustSound, missileSound, shipInfo,
                                        new Vector2(
                                        graphics.PreferredBackBufferWidth / 2,
                                        graphics.PreferredBackBufferHeight / 2));
                        started = false;
                        IsMouseVisible = true;
                    }
                }
            }

            // else activate mouse and splashscreen, wait for the click
            else
            {
                MouseState mouse = Mouse.GetState();
                if (mouse.LeftButton == ButtonState.Pressed &&
                    mouse.X > 200 && mouse.X < 600 && mouse.Y > 150 && mouse.Y < 450)
                {
                    // here we go, start a new game
                    lives = 3;
                    score = 0;
                    IsMouseVisible = false;
                    started = true;
                    soundtrack.Resume();
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

            // DONE: Add your drawing code here
            spriteBatch.Begin();

            // draw the background
            spriteBatch.Draw(nebula, nebulaRectangle, Color.White);
            spriteBatch.Draw(debris, debrisRectangle1, Color.White);
            spriteBatch.Draw(debris, debrisRectangle2, Color.White);

            // draw the ship
            ship.Draw(spriteBatch);

            // draw sprite groups
            DrawSpriteGroup(spriteBatch, missileGroup);
            DrawSpriteGroup(spriteBatch, asteroidGroup);
            DrawSpriteGroup(spriteBatch, explosionGroup);

            // Draw texts
            DrawTexts(spriteBatch);

            // draw the splash screen only while waiting for a new game
            if (!started) spriteBatch.Draw(splash, splashRectangle, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        #region Public methods

        /// <summary>
        /// Adds the given missile to the game
        /// </summary>
        /// <param name="position">the position of the missile to add</param>
        /// <param name="velocity">the velocity of the missile to add</param>
        /// /// <param name="angle">the angle of the missile to add</param>
        public static void AddMissile(Vector2 position, Vector2 velocity, float angle)
        {
            Sprite missile = new Sprite(missileSprite, position, velocity, angle, 0, missileInfo);
            missileGroup.Add(missile);
        }

        /// <summary>
        /// Computes the distance of two points
        /// </summary>
        /// <param name="pos1">position of the first point</param>
        /// <param name="pos2">position of the second point</param>
        /// <returns> the distance</returns>
        public static float Distance(Vector2 pos1, Vector2 pos2)
        {
            float distance = (float)Math.Sqrt(Math.Pow(pos1.X - pos2.X, 2) + Math.Pow(pos1.Y - pos2.Y, 2));
            return distance;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Adds the random random random asteroid to the game
        /// </summary>
        /// <param name="shipPos">the position of the ship</param>
        private void SpawnAsteroid(Vector2 shipPos)
        {
            // generate random sprite, position, velocity and velocity of rotation
            int idx = RandomNumberGenerator.Next(4);
            Texture2D asteroidSprite = ASTEROID_SPRITES[idx];
            Vector2 pos = shipPos;

            // any asteroid can't be spawned too close to the ship
            while (Distance(shipPos, pos) < Constants.SHIP_SAFE_DIST)
            {
                int posX = RandomNumberGenerator.Next(Constants.WINDOW_WIDTH);
                int posY = RandomNumberGenerator.Next(Constants.WINDOW_HEIGHT);
                pos = new Vector2(posX, posY);
            }

            float speed = RandomNumberGenerator.NextFloat(
                          Constants.ASTEROID_MIN_VEL, Constants.ASTEROID_MAX_VEL);
            float angle = RandomNumberGenerator.NextFloat(0, (float)Math.PI);
            float angleVel = RandomNumberGenerator.NextFloat(
                            -Constants.ASTEROID_ANGLE_VEL, Constants.ASTEROID_ANGLE_VEL);
            Vector2 vel = new Vector2(
                            speed * (float)Math.Sin(angle),
                            speed * (float)Math.Cos(angle));

            Sprite newAsteroid = new Sprite(asteroidSprite, pos, vel, 0, angleVel, asteroidInfo);
            asteroidGroup.Add(newAsteroid);
        }

        /// <summary>
        /// Upgrades sprites in the group and removes if inactive
        /// </summary>
        /// <param name="gameTime">game time</param>
        /// <param name="spriteGroup">the group of sprites</param>
        private void ProcessSpriteGroup(GameTime gameTime, List<Sprite> spriteGroup)
        {
            for (int i = spriteGroup.Count - 1; i >= 0; i--)
            {
                spriteGroup[i].Update(gameTime);
                if (!spriteGroup[i].Active)
                    spriteGroup.RemoveAt(i);
            }
        }

        /// <summary>
        /// removes destroyed sprites that collided with the other_object and starts explosion
        /// </summary>
        /// <param name="spriteGroup">the group of sprites</param>
        /// <param name="otherObject">the other sprite</param>
        /// <returns>true if at least one sprite was destroyed</returns>
        private bool GroupColide(List<Sprite> spriteGroup, Sprite otherObject, SoundEffect explosionSound)
        {
            bool isDestroyed = false;
            for (int i = spriteGroup.Count - 1; i >= 0; i--)
            {
                if (spriteGroup[i].Colide(otherObject))
                {
                    isDestroyed = true;
                    int index = RandomNumberGenerator.Next(4);
                    Texture2D explosionSprite = EXPLOSION_TILES[index];
                    Sprite explosion = new Sprite(
                        explosionSprite,
                        spriteGroup[i].Position,
                        Vector2.Zero, 0, 0,
                        explosionInfo);
                    explosionSound.Play();
                    explosionGroup.Add(explosion);
                    spriteGroup.RemoveAt(i);
                }
            }
            return isDestroyed;
        }

        /// <summary>
        /// removes destroyed sprites that collided with the ship and starts explosion
        /// </summary>
        /// <param name="spriteGroup">the group of sprites</param>
        /// <param name="otherObject">the other sprite</param>
        /// <returns>true if at least one sprite was destroyed</returns>
        private bool ShipColide(List<Sprite> spriteGroup, SoundEffect explosionSound)
        {
            bool isDestroyed = false;
            for (int i = spriteGroup.Count - 1; i >= 0; i--)
            {
                if (spriteGroup[i].shipColide(ship))
                {
                    isDestroyed = true;
                    int index = RandomNumberGenerator.Next(4);
                    Texture2D explosionSprite = EXPLOSION_TILES[index];
                    Sprite explosion = new Sprite(
                        explosionSprite,
                        spriteGroup[i].Position,
                        Vector2.Zero, 0, 0,
                        explosionInfo);
                    explosionSound.Play();
                    explosionGroup.Add(explosion);
                    spriteGroup.RemoveAt(i);
                }
            }
            return isDestroyed;
        }

        /// <summary>
        /// removes destroyed sprites from first group if collided with any sprite
        /// from second group
        /// *returns number of destroyed pairs
        /// </summary>
        /// <param name="spriteGroup">the group of sprites</param>
        /// <returns> the distance</returns>
        private int GroupGroupColide(List<Sprite> firstGroup, List<Sprite> secondGroup, SoundEffect explosionSound)
        {
            int numDestroyed = 0;
            for (int i = firstGroup.Count - 1; i >= 0; i--)
            {
                if (GroupColide(secondGroup, firstGroup[i], explosionSound))
                {
                    firstGroup.RemoveAt(i);
                    numDestroyed++;
                }
            }
            return numDestroyed;
        }

        /// <summary>
        /// Draws sprites in the group
        /// </summary>
        /// <param name="spriteBatch">game time</param>
        /// <param name="spriteGroup">the group of sprites</param>
        private void DrawSpriteGroup(SpriteBatch spriteBatch, List<Sprite> spriteGroup)
        {
            foreach (Sprite sprite in spriteGroup)
                sprite.Draw(spriteBatch);
        }

        /// <summary>
        /// Draws all texts
        /// </summary>
        /// <param name="spriteBatch">spriteBatch</param>
        private void DrawTexts(SpriteBatch spriteBatch)
        {
            // Draw Score
            scoreStr = "Score\n" + score.ToString();
            fontOrigin = font.MeasureString(scoreStr) / 2;
            spriteBatch.DrawString(font, scoreStr, scorePos, Color.White,
                0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);

            livesStr = "Lives\n" + lives.ToString();
            fontOrigin = font.MeasureString(livesStr) / 2;
            spriteBatch.DrawString(font, livesStr, livesPos, Color.White,
                0, fontOrigin, 1.0f, SpriteEffects.None, 0.5f);
        }

        #endregion
    }
}
