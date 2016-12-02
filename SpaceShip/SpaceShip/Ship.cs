using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceShip
{
    /// <summary>
    /// A spaceship
    /// </summary>
    class Ship
    {
        #region Fields

        // drawing support
        Texture2D sprite;
        Point size;
        Rectangle sourceRectangle;
        Vector2 origin;
 
        // moving support
        Vector2 position, velocity, thrustVelocity;
        bool thrust = false;
        float angle, angleVelocity;
        int radius;

        // shooting support
        Vector2 gunTip;
        bool canShoot = true;
        int elapsedCooldownTime = 0;

        // sound support
        SoundEffectInstance thrustSound;
        SoundEffect shootSound;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="sprite">a sprite for the ship</param>
        /// <param name="thrustSound">a sound of thrust</param>
        /// <param name="position">the position of the center of the ship</param>
        /// <param name="info">info of the image of the ship</param>
        public Ship(Texture2D sprite, SoundEffectInstance thrustSound, SoundEffect shootSound, ImageInfo info, Vector2 position)
        {
            // sets initial values
            this.sprite = sprite;
            this.thrustSound = thrustSound;
            this.shootSound = shootSound;
            this.position = position;
            size = info.Size;
            velocity = new Vector2(0, 0);
            sourceRectangle = new Rectangle(0, 0, size.X, size.Y);
            origin = info.Origin;
            radius = info.Radius;
         }

        #endregion


        #region Properties

        /// <summary>
        /// Gets the position of the ship
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
        }

        /// <summary>
        /// Gets the radius of the ship to determine collision
        /// </summary>
        public int Radius
        {
            get { return radius; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the ship
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime, KeyboardState key)
        {
            // updates the ship thrust
            if (key.IsKeyDown(Keys.Up))
            {
                thrust = true;
                thrustSound.Resume();
            }
            else
            {
                thrust = false;
                thrustSound.Pause();
            }

            // updates the ship rotation
            if (key.IsKeyDown(Keys.Left)) angleVelocity = -Constants.ANGLE_VEL;
            else if (key.IsKeyDown(Keys.Right)) angleVelocity = Constants.ANGLE_VEL;
            else angleVelocity = 0;

            // update shooting allowed
            if (!canShoot)
            {
                elapsedCooldownTime += gameTime.ElapsedGameTime.Milliseconds;

                // timer concept (for animations) introduced in Chapter 7
                if (elapsedCooldownTime > Constants.MISSILE_COOLDOWN_MILLISECONDS
                    || key.IsKeyUp(Keys.Space))
                {
                    canShoot = true;
                    elapsedCooldownTime = 0;
                }
            }

            // update angle
            angle += angleVelocity * gameTime.ElapsedGameTime.Milliseconds;

            // update velocity due to friction and thrust
            if (thrust) thrustVelocity = Rectangular(new Vector2(1, angle)) * Constants.SHIP_SPEED;
            else thrustVelocity = Vector2.Zero;
            velocity = (1 - Constants.FRICTION) * velocity + (thrustVelocity);

            // uprates position and drawing stuff
            position.X += velocity.X * gameTime.ElapsedGameTime.Milliseconds;
            position.Y += velocity.Y * gameTime.ElapsedGameTime.Milliseconds;

            // if out of window, reveal the ship on the oposite side
            if (position.X < 0) position.X = Constants.WINDOW_WIDTH;
            else position.X = position.X % Constants.WINDOW_WIDTH;
            if (position.Y < 0) position.Y = Constants.WINDOW_HEIGHT;
            else position.Y = position.Y % Constants.WINDOW_HEIGHT;

            // shoot if can
            if (canShoot && key.IsKeyDown(Keys.Space)) Shoot();
        }

        /// <summary>
        /// Draws the ship
        /// </summary>
        /// <param name="spriteBatch">sprite batch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // use the sprite batch to draw the ship
            if (thrust) sourceRectangle.X = sprite.Width / 2;
            else sourceRectangle.X = 0;
            spriteBatch.Draw(sprite, position, sourceRectangle, Color.White, angle,
                origin, 1.0f, SpriteEffects.None, 0f);
        }

        /// <summary>
        /// create and return a new missile
        /// </summary>
        /// <param name="missileSprite">the missile sprite</param>
        /// <returns>a missile</returns>
        public void Shoot()
        {
            // create a new missile
            Vector2 offset = new Vector2(radius, angle);
            gunTip = position + Rectangular(offset);
            Vector2 missileVelocity = new Vector2();
            missileVelocity = (gunTip - position) * Constants.MISSILE_SPEED + velocity;
            Game1.AddMissile(gunTip, missileVelocity, angle);
            shootSound.Play();
            canShoot = false;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Transforms a vector from polar coords (r, phi) to rectangular coords (x, y)
        /// </summary> 
        /// <param name="polar">vector in polar coordinates</param>
        /// <returns>vector in rectangular coordinates</returns>
        private Vector2 Rectangular(Vector2 polar)
        {
            return new Vector2(polar.X * (float) Math.Cos(polar.Y), polar.X * (float) Math.Sin(polar.Y));
        }

        /// <summary>
        /// Transforms a vector from rectangular coords (x, y) to polar coords (r, phi)
        /// </summary> 
        /// <param name="polar">vector in rectangular coordinates</param>
        /// <returns>vector in polar coordinates</returns>
        private Vector2 Polar(Vector2 rect)
        {
            return new Vector2(rect.Length(), (float)Math.Atan2(rect.Y , rect.X));
        }

        #endregion
    }
}
