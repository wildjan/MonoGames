using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    /// <summary>
    /// An Arena of 2D grid of cells
    /// </summary>
    public abstract class Grid
    {
        #region Fields

        // dimensions of the Grid
        int numRows;
        int numCols;

        // a field of cells
        bool[,] cells;

        // randomness support
        Random rnd = new Random();

        #endregion

        #region Constructors

        public Grid(int numRows, int numCols)
        {
            this.numRows = numRows;
            this.numCols = numCols;
            cells = new bool[NumRows, numCols];
            this.Clear();
        }

        #endregion

        #region Properties

        public int NumRows
        {
            get { return numRows; }
        }

        public int NumCols
        {
            get { return numCols; }
        }

        #endregion

        #region Public methods

        public void Clear()
        {
            for (int row = 0; row < numRows; row++)
                for (int col = 0; col < numCols; col++)
                    cells[row, col] = false;
        }

        public void EmptyCell(Position position)
        {
            cells[position.Row, position.Col] = false;
        }

        public void FullCell(Position position, Position cell)
        {
            cells[position.Row, position.Col] = true;
        }

        public List<Position> FourNeighbors(Position position)
        {
            List<Position> neighbours = new List<Position>();

            if (position.Row > 0) neighbours.Add(new Position(position.Row - 1, position.Col));
            if (position.Row < numRows) neighbours.Add(new Position(position.Row + 1, position.Col));
            if (position.Col > 0) neighbours.Add(new Position(position.Row, position.Col - 1));
            if (position.Col < numCols) neighbours.Add(new Position(position.Row, position.Col + 1));

            return neighbours;
        }

        public Position TailPosition(Position position, Vector2 direction)
        {
            position.Row += (int) -direction.Y;
            position.Col += (int) -direction.X;

            return position;
        }

        public Position NextPosition(Position position, Vector2 direction)
        {
            position.Row += (int) direction.Y;
            position.Col += (int) direction.X;

            return position;
        }

        public Position GetRandomPosition()
        {
            Position position = new Position(0, 0);
            position.Row = rnd.Next(numRows);
            position.Col = rnd.Next(numCols);

            while (!IsEmpty(position))
            {
                position.Row = rnd.Next(numRows);
                position.Col = rnd.Next(numCols);
            }

            return position;
        }


        #endregion

        #region Private methods

        private bool IsEmpty(Position position)
        {
            return !cells[position.Row, position.Col];
        }
        #endregion
    }
}
