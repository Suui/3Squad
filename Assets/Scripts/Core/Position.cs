using UnityEngine;


namespace Medusa
{

    public sealed class Position
    {

        private readonly int row, column;


        public Position(int row, int column)
        {
            this.row = row;
            this.column = column;
        }


        public Direction GetDirectionTo(Position position)
        {
            return new Direction(position.row - row, position.column - column);
        }

		public bool Outside(Layer layer)
		{
			return layer.Outside(this);
		}


        public int GetDistanceTo(Position position)
        {
            return GetDirectionTo(position).Magnitude;
        }


        #region Operators

        // Operator ==
        public static bool operator ==(Position a, Position b)
        {
            // If both are null, or both are same instance, return true.
            if (ReferenceEquals(a, b))
                return true;

            if (((object)a == null) || ((object)b == null))
                return false;

            return a.row == b.row && a.column == b.column;
        }


        // Operator !=
        public static bool operator !=(Position a, Position b)
        {
            return !(a == b);
        }


        // Operator + Position
        public static Position operator +(Position a, Position b)
        {
            return new Position(a.row + b.row, a.column + b.column);
        }


        // Operator - Position
        public static Position operator -(Position a, Position b)
        {
            return new Position(a.row - b.row, a.column - b.column);
        }


        // Operator + Direction
        public static Position operator +(Position a, Direction b)
        {
            return new Position(a.row + b.Row, a.column + b.Column);
        }


        // Operator - Direction
        public static Position operator -(Position a, Direction b)
        {
            return new Position(a.row - b.Row, a.column - b.Column);
        }

        #endregion


        #region Castings

        // Casting Position to Vector3
        public static implicit operator Vector3(Position position)
        {
            return new Vector3(position.row, 0, position.column);
        }


        // Casting Vector3 to Position
        public static explicit operator Position(Vector3 vector3)
        {
            return new Position(Mathf.FloorToInt(vector3.x), Mathf.FloorToInt(vector3.z));
        }


        // TODO: Maybe not useful
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

            return (row == position.row) && (column == position.column);
        }


        public bool Equals(Position position)
        {
            if ((object)position == null)
                return false;

            return (row == position.row) && (column == position.column);
        }


        public override int GetHashCode()
        {
            return 31 * (31 + row) + column;
        }


        public override string ToString()
        {
            return "(" + row + ", " + column + ")";
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

        #endregion

    }
    
}