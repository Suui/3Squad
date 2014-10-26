using System;
using UnityEngine;
using System.Collections.Generic;

namespace Medusa
{

    public sealed class Position
    {

        public static int MAX_COLUMNS = 100;
        public int x, z;

        private static int ASCII_OFFSET = 97;


        public Position(int x, int z)
        {
            this.x = x;
            this.z = z;
        }


        #region Utilities

        public Direction GetDirectionTo(Position position)
        {
            return new Direction(x - position.x, z - position.z);
        }


        public int GetDistanceTo(Position position)
        {
            return GetDirectionTo(position).Length;
        }


        // Set new name
        public IEnumerable<Position> Ray(Direction direction, Dimension bounds, int range = Int32.MaxValue)
        {
            Position pos = this;
            while ((pos += direction).Inside(bounds) && range-- > 0)
            {
                yield return pos;
            }
        }


        public static IEnumerable<Position> Range(Dimension dim)
        {
            for (int row = 0; row < dim.Rows; row++)
            {
                for (int column = 0; column < dim.Columns; column++)
                {
                    yield return new Position(row, column);
                }
            }
        }


        #endregion


        #region Operators

        // Operator ==
        public static bool operator ==(Position a, Position b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.x == b.x && a.z == b.z;
        }


        // Operator !=
        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }


        // Operator + Position
        public static Position operator +(Position a, Position b)
        {
            return new Position(a.x + b.x, a.z + b.z);
        }


        // Operator - Position
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.x - b.x, a.z - b.z);
        }


        // Operator + Direction
        public static Position operator +(Position a, Direction b)
        {
            return new Position(a.x + b.x, a.z + b.z);
        }


        // Operator - Direction
        public static Position operator -(Position a, Direction b)
        {
            return new Position(a.x - b.x, a.z - b.z);
        }

        #endregion


        #region Castings

        // Casting Position to Vector3
        public static implicit operator Vector3(Position position)
        {
            return new Vector3(position.x, 0, position.z);
        }


        // Casting Vector3 to Position
        public static explicit operator Position(Vector3 vector3)
        {
            return new Position(Mathf.FloorToInt(vector3.x), Mathf.FloorToInt(vector3.z));
        }


        // Casting Transform to Position
        public static explicit operator Position(Transform transform)
        {
            return (Position) transform.position;
        }

        #endregion


        #region Equals, GetHashCode, ToString Overrides

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            // If parameter cannot be cast to Position return false.
            Position position = obj as Position;
            if ((System.Object)position == null)
                return false;

            return (x == position.x) && (z == position.z);
        }


        public bool Equals(Position position)
        {
            if ((object)position == null)
                return false;

            return (x == position.x) && (z == position.z);
        }


        public override int GetHashCode()
        {
            return x ^ z;
        }


        public override string ToString()
        {
            return x + ", " + z;
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

        #endregion

    }
    
}