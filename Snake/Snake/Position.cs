using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snake
{
    public struct Position
    {
        #region Fields

        // a position fields
        private int row, col;

        #endregion

        #region Constructors

        public Position(int row, int col)
        {
            this.row = row;
            this.col = col;
        }

        #endregion

        #region Properties

        public int Row
        {
            get { return row; }
            set { row = value; }
        }

        public int Col
        {
            get { return col; }
            set { col = value; }
        }

        #endregion

        #region Public methods

        #endregion

    }
}
