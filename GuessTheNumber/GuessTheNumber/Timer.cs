using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace GuessTheNumber
{
    /// <summary>
    /// A timer
    /// </summary>
    public class Timer
    {
        #region Fields

        bool running = false;
        int totalMilliseconds;
        int elapsedMilliseconds;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="milliseconds">the milliseconds for the timer</param>
        public Timer(int milliseconds)
        {
            this.totalMilliseconds = milliseconds;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the timer is running 
        /// </summary>
        public bool IsRunning
        {
            get { return running; }
        }

        /// <summary>
        /// Gets miliseconds elapsed
        /// </summary>
        public int ElapsedMilliseconds
        {
            get { return elapsedMilliseconds; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the timer
        /// </summary>
        /// <param name="gameTime">the game time</param>
        public void Update(GameTime gameTime)
        {
            if (running)
            {
                elapsedMilliseconds += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedMilliseconds >= totalMilliseconds)
                {
                    running = false;
                }
            }
        }

        /// <summary>
        /// Starts the timer
        /// </summary>
        public void Start()
        {
            running = true;
            elapsedMilliseconds = 0;
        }

        #endregion
    }
}
