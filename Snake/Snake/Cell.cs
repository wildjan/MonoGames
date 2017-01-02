using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// A cell
    /// </summary>
    public class Cell
    {
        #region Fields

        // Constants
        
        // location support
        Position position; 

        // movement support
        const int BaseSpeed = 2;
        Vector2 direction;
        Vector2 oldDirection;

        // drawing support
        Texture2D sprite;
        Rectangle drawRectangle = new Rectangle(0, 0, GameConstants.CellSize, GameConstants.CellSize);

        #endregion

        #region Constructors

        public Cell(Position position, Vector2 direction, ContentManager contentManager, string spriteName)
        {
            // set location
            this.position = position;
            drawRectangle.X = position.Col * GameConstants.CellSize;
            drawRectangle.Y = position.Row * GameConstants.CellSize;

            // set velocities
            this.direction = direction;
            oldDirection = direction;

            // set sprite and draw rectangle
            sprite = contentManager.Load<Texture2D>(@"graphics\" + spriteName);
        }

        #endregion

        #region Properties

        public Position Position
        {
            get { return position; }
        }

        public Vector2 Direction
        {
            get { return direction; }
        }

        public Vector2 OldDirection
        {
            get { return oldDirection; }
        }

        #endregion

        #region Private methods

        #endregion

        #region Public methods

        public void Update(Vector2 nextDirection)
        {
            oldDirection = direction;
            direction = nextDirection;

            position.Row += (int) nextDirection.Y;
            position.Col += (int) nextDirection.X;
            drawRectangle.X = position.Col * GameConstants.CellSize;
            drawRectangle.Y = position.Row * GameConstants.CellSize;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(sprite, drawRectangle, Color.White);
        }

        #endregion
    }
}
