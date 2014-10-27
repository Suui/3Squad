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

        private readonly int x, z, magnitude;   // Manhattan magnitud


        public Direction(int x, int z)
        {
            this.x = x;
            this.z = z;
            magnitude = Mathf.Abs(x) + Mathf.Abs(z);
        }


        #region Operators

        // Operator ==
        public static bool operator ==(Direction a, Direction b)
        {
            return a.x == b.x && a.z == b.z;
        }


        // Operator !=
        public static bool operator !=(Direction a, Direction b)
        {
            return a.x != b.x || a.z != b.z;
        }


        // Operator + Direction
        public static Direction operator +(Direction a, Direction b)
        {
            return new Direction(a.x + b.x, a.z + b.z);
        }


        // Operator - Direction
        public static Direction operator -(Direction a, Direction b)
        {
            return new Direction(a.x - b.x, a.z - b.z);
        }


        // Operator + Position
        public static Direction operator +(Direction a, Position b)
        {
            return new Direction(a.x + b.X, a.z + b.Z);
        }


        // Operator - Position
        public static Direction operator -(Direction a, Position b)
        {
            return new Direction(a.x - b.X, a.z - b.Z);
        }


        // Operator opposed
        public static Direction operator -(Direction dir)
        {
            return new Direction(-dir.x, -dir.z);
        }


        // Operator * int
        public static Direction operator *(Direction dir, int scalar)
        {
            return new Direction(dir.x * scalar, dir.z * scalar);
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
            return x ^ z;
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


        public int Magnitude
        {
            get { return magnitude; }
        }

        #endregion


    }
}