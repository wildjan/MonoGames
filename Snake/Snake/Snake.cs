using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// A Snake
    /// </summary>
    class Snake : Grid
    {
        #region Fields

        // a snake as a list of cells
        List<Cell> snake = new List<Cell>();

        // random support
        Random rnd = new Random();

        // a target support
        Position targetPosition;
        Cell target;

        // the snake prolongation support
        bool adding = false;
        int celsAdded = 0;

        ContentManager contentManager;


        #endregion

        #region Constructors

        public Snake(int numRows, int numCols, ContentManager contentManager) : base(numRows, numCols)
        {
            this.contentManager = contentManager;

            // create a new Snake with a head random position and random direction
            Position position = GetRandomPosition();
            Vector2 direction = GetRandomDirection();
            snake.Add(new Cell(position, direction, contentManager, "white"));
            AddTail();

            SetTarget();
        }

        #endregion

        #region Public methods

        public void Update(KeyboardState keyboard)
        {
            Vector2 direction = snake[0].Direction;
            Position headPosition = snake[0].Position;
            Vector2 nextDirection = snake[0].Direction;

            if (direction == nextDirection)
            { 
                if (direction != GameConstants.RIGHT &&
                    direction != GameConstants.LEFT &&
                    (keyboard.IsKeyDown(Keys.Right) ||
                    keyboard.IsKeyDown(Keys.D)))
                {
                    nextDirection = GameConstants.RIGHT;

                }
                if (direction != GameConstants.LEFT &&
                    direction != GameConstants.RIGHT &&
                    (keyboard.IsKeyDown(Keys.Left) ||
                    keyboard.IsKeyDown(Keys.A)))
                {
                    nextDirection = GameConstants.LEFT;
                }
                if (direction != GameConstants.UP &&
                    direction != GameConstants.DOWN &&
                    (keyboard.IsKeyDown(Keys.Up) ||
                    keyboard.IsKeyDown(Keys.W)))
                {
                    nextDirection = GameConstants.UP;
                }
                if (direction != GameConstants.DOWN &&
                    direction != GameConstants.UP &&
                    (keyboard.IsKeyDown(Keys.Down) ||
                    keyboard.IsKeyDown(Keys.S)))
                {
                    nextDirection = GameConstants.DOWN;
                }
            }

            Position headNextPosition = NextPosition(headPosition, nextDirection);

            if (headNextPosition.Row == target.Position.Row && headNextPosition.Col == target.Position.Col)
            {
                SetTarget();
                adding = true;
            }

            snake[0].Update(nextDirection);
            for (int i = 1; i < snake.Count; i++)
            {
                nextDirection = snake[i - 1].OldDirection;
                snake[i].Update(nextDirection);
            }

            if (adding && celsAdded < 5)
            {
                AddTail();
                celsAdded += 1;
            }
            else
            {
                celsAdded = 0;
                adding = false;
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cell cell in snake)
            {
                cell.Draw(spriteBatch);
            }

            target.Draw(spriteBatch);
        }


        #endregion

        #region Private methods

        Vector2 GetRandomDirection()
        {
            Vector2 direction = GameConstants.RIGHT;
            //int index = rnd.Next(4);

            //if (index == 0) direction = GameConstants.UP;
            //else if (index == 1) direction = GameConstants.DOWN;
            //else if (index == 2) direction = GameConstants.LEFT;
            //else if (index == 3) direction = GameConstants.RIGHT;
            return direction;
        }

        private void SetTarget()
        {
            targetPosition = GetRandomPosition();
            target = new Cell(targetPosition, new Vector2(0, 0), contentManager, "red");
        }

        private void AddTail()
        {
            Cell lastCell = snake[snake.Count - 1];
            Position lastCellPosition = lastCell.Position;
            Vector2 lastCellDirection = lastCell.Direction;
            snake.Add(new Cell(TailPosition(lastCellPosition, lastCellDirection), lastCellDirection, contentManager, "white"));
        }

        #endregion
    }
}

