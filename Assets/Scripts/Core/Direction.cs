using UnityEngine;


namespace Medusa
{

    public struct Direction
    {

        public static readonly Direction Left = new Direction(-1, 0);
        public static readonly Direction Right = -Left;
        public static readonly Direction Up = new Direction(0, 1);
        public static readonly Direction Down = -Up;

        public static readonly Direction[] AllStaticDirections = { Up, Right, Down, Left };

        private readonly int row, column, magnitude;   // Manhattan magnitud


        public Direction(int row, int column)
        {
            this.row = row;
            this.column = column;
            magnitude = Mathf.Abs(row) + Mathf.Abs(column);
        }


        #region Operators

        // Operator ==
        public static bool operator ==(Direction a, Direction b)
        {
            return a.row == b.row && a.column == b.column;
        }


        // Operator !=
        public static bool operator !=(Direction a, Direction b)
        {
            return a.row != b.row || a.column != b.column;
        }


        // Operator + Direction
        public static Direction operator +(Direction a, Direction b)
        {
            return new Direction(a.row + b.row, a.column + b.column);
        }


        // Operator - Direction
        public static Direction operator -(Direction a, Direction b)
        {
            return new Direction(a.row - b.row, a.column - b.column);
        }


        // Operator + Position
        public static Direction operator +(Direction a, Position b)
        {
            return new Direction(a.row + b.Row, a.column + b.Column);
        }


        // Operator - Position
        public static Direction operator -(Direction a, Position b)
        {
            return new Direction(a.row - b.Row, a.column - b.Column);
        }


        // Operator opposed
        public static Direction operator -(Direction dir)
        {
            return new Direction(-dir.row, -dir.column);
        }


        // Operator * int
        public static Direction operator *(Direction dir, int scalar)
        {
            return new Direction(dir.row * scalar, dir.column * scalar);
        }

        #endregion


        #region Equals, GetHasCode Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return this == (Direction)obj;
        }


        public bool Equals(Direction direction)
        {
            return this == direction;
        }


        public override int GetHashCode()
        {
            return row ^ column;
        }

        #endregion


        #region Getters

        public int Row
        {
            get { return row; }
        }


        public int Column
        {
            get { return column; }
        }


        public int Magnitude
        {
            get { return magnitude; }
        }

        #endregion


    }
}