using System;
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

        private int x, z, length;


        public Direction(int x, int z)
        {
            this.x = x;
            this.z = z;
            length = Math.Abs(x) + Math.Abs(z);
        }


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
            return Columns * Position.MAX_COLUMNS + Rows;
        }

        #endregion


        #region Getters

        public int X
        {
            get { return x; }
        }


        public int Z
        {
            get { return z; }
        }


        public int Length
        {
            get { return length; }
        }

        #endregion


    }
}