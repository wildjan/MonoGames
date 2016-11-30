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
    /// A class for a front end menu
    /// </summary>
    class Menu
    {
        #region Fields

        // fields for buttons
        MenuButton playButton;
        MenuButton magEButton;
        MenuButton quitButton;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="contentManager">the content manager for loading content</param>
        /// <param name="windowWidth">the window width</param>
        /// <param name="windowHeight">the window height</param>
        public Menu(ContentManager contentManager, int windowWidth, int windowHeight)
        {
            // used for button placement
            int centerX = 170;
            int topCenterY = 200;
            Vector2 buttonCenter = new Vector2(centerX, topCenterY);

            // create buttons
            playButton = new MenuButton(contentManager.Load<Texture2D>(@"graphics\playbutton"),
                buttonCenter, GameState.Play);
            buttonCenter.Y += 100;
            magEButton = new MenuButton(contentManager.Load<Texture2D>(@"graphics\mageyebutton"),
                buttonCenter, GameState.MagEye);
            buttonCenter.Y += 100;
            quitButton = new MenuButton(contentManager.Load<Texture2D>(@"graphics\quitbutton"),
                buttonCenter, GameState.Quit);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Updates the menu
        /// </summary>
        /// <param name="mouse">the current mouse state</param>
        public void Update(MouseState mouse, bool gameRunning)
        {
            // update buttons
            magEButton.Update(mouse);
            if (!gameRunning)
            {
                playButton.Update(mouse);
                quitButton.Update(mouse);
            }
        }

        /// <summary>
        /// Draws the menu
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // draw buttons
            playButton.Draw(spriteBatch);
            magEButton.Draw(spriteBatch);
            quitButton.Draw(spriteBatch);
        }

        #endregion
    }
}
