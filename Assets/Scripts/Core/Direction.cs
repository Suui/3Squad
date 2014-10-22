using System;

namespace Medusa
{

    public struct Direction
    {

        #region Basic Properties

        public int Rows
        {
            get;
            private set;
        }

        public int Columns
        {
            get;
            private set;
        }

        public int Length
        {
            get;
            private set;
        }

        #endregion

        #region Static Stuff

        public static readonly Direction Left = new Direction(-1, 0);
        public static readonly Direction Right = -Left;
        public static readonly Direction Up = new Direction(0, 1);
        public static readonly Direction Down = -Up;

        public static readonly Direction[] All = {Up,Right,Down,Left};

        #endregion

        #region Constructor

        public Direction(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
            Length = Math.Abs(rows) + Math.Abs(columns);
        }

        #endregion

        #region Operators

        #region Equality

        public static bool operator ==(Direction a, Direction b)
        {
            return a.Rows == b.Rows && a.Columns == b.Columns;
        }

        public static bool operator !=(Direction a, Direction b)
        {
            return a.Rows != b.Rows || a.Columns != b.Columns;
        }

        #endregion

        #region Arithmetic

        public static Direction operator +(Direction a, Direction b)
        {
            return new Direction(a.Rows + b.Rows, a.Columns + b.Columns);
        }
        
        public static Direction operator -(Direction a, Direction b)
        {
            return new Direction(a.Rows - b.Rows, a.Columns - b.Columns);
        }

        public static Direction operator -(Direction dir)
        {
            return new Direction(-dir.Rows, -dir.Columns);
        }

        public static Direction operator *(Direction dir, int scalar)
        {
            return new Direction(dir.Rows * scalar, dir.Columns * scalar);
        }

        #endregion

        #endregion

        #region System.Object Override

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return this == (Direction)obj;
        }

        public override int GetHashCode()
        {
            return Columns * 100 + Rows;
        }

        #endregion

    }
}