using System;
using UnityEngine;
using System.Collections.Generic;

namespace Medusa
{

    public sealed class Position
    {

        #region Static Stuff

        private static int ASCII_OFFSET = 97;

        #endregion

        #region Basic Properties

        public int Row
        {
            get;
            private set;
        }

        public int Column
        {
            get;
            private set;
        }

        #endregion

        #region Constructor

        public Position(int row, int column)
        {
            Row = row;
            Column = column;
        }

        #endregion

		#region Operators

        #region Equality

        public static bool operator ==(Position a, Position b)
        {
            if ((object)a == null)
                return (object)b == null;
            if ((object)b == null)
                return (object)a == null;
            return (a.Column == b.Column) && (a.Row == b.Row);
        }

        public static bool operator !=(Position a, Position b)
        {
            if ((object)a == null)
                return (object)b != null;
            if ((object)b == null)
                return (object)a != null;
            return (a.Column != b.Column) || (a.Row != b.Row);
        }

        #endregion

        #region Arithmetic

        public static Position operator +(Position pos, Direction dir)
        {
            return new Position(pos.Column + dir.Columns, pos.Row + dir.Rows);
        }

        public static Position operator -(Position pos, Direction dir)
        {
            return new Position(pos.Column - dir.Columns, pos.Row - dir.Rows);
        }

        #endregion

        #region Conversion

        public static implicit operator Vector3(Position pos)
        {
            return new Vector3(pos.Column, 0, pos.Row);
        }

        public static explicit operator Position(Vector3 vec)
        {
            return new Position(Convert.ToInt32(vec.z), Convert.ToInt32(vec.x));
        }

        public static explicit operator Position(String str)
        {
            str = str.ToLower();
            return new Position(Int32.Parse(str.Substring(1)) - 1, (int)str [0] - ASCII_OFFSET);
        }

        public static explicit operator Position(Transform tr)
        {
            return (Position)tr.position;
        }

        public static explicit operator Position(GameObject go)
        {
            return (Position)go.transform;
        }

        #endregion

        #endregion

        #region System.Object Override

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return this == (Position)obj;
        }

        public override int GetHashCode()
        {
            return Column * 100 + Row;
        }

        public override string ToString()
        {
            return char.ConvertFromUtf32(Column + ASCII_OFFSET) + (Row + 1);
        }

        #endregion

        #region Bound Checking
        
        public bool Inside(Dimension dim)
        {
            return Row >= 0 && Row < dim.Rows
                && Column >= 0 && Column < dim.Columns;
        }
        
        public bool Outside(Dimension dim)
        {
            return Row < 0 || Row >= dim.Rows
                || Column < 0 || Column >= dim.Columns;
        }
        
        #endregion

        #region Utilities

        public Direction To(Position other)
        {
            return new Direction(this.Column - other.Column, this.Row - other.Row);
        }
        
        public int Distance(Position other)
        {
            return this.To(other).Length;
        }
        
        public IEnumerable<Position> Ray(Direction dir, Dimension bounds, int range = Int32.MaxValue)
        {
            Position pos = this;
            while ((pos+=dir).Inside(bounds) && range-- > 0)
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
        
    }
    
}