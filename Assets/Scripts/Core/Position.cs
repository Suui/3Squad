using System;

namespace Medusa.Core
{

  public sealed class Position
  {

    public readonly int x;
    public readonly int y;
    public static int ASCII = 65;

    public Position(int x, int y)
    {
      this.x = x;
      this.y = y;
    }

    // Comparaciones
    public static bool operator ==(Position a, Position b)
    {
      if ((object)a == null)
        return (object)b == null;
      if ((object)b == null)
        return (object)a == null;
      return (a.x == b.x) && (a.y == b.y);
    }

    public static bool operator !=(Position a, Position b)
    {
      if ((object)a == null)
        return (object)b != null;
      if ((object)b == null)
        return (object)a != null;
      return (a.x != b.x) || (a.y != b.y);
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      return this == (Position)obj;
    }

    public override int GetHashCode()
    {
      return x * 100 + y;
    }

    public static Position operator +(Position position, Direction direction)
    {
      return new Position(position.x + direction.x, position.y + direction.y);
    }

    public Direction To(Position other)
    {
      return new Direction(this.x - other.x, this.y - other.y);
    }

    public int Distance(Position other)
    {
      return this.To(other).length;
    }

    public override string ToString()
    {
      return char.ConvertFromUtf32(x + ASCII) + (y + 1);
    }
  
    public static Position FromString(string str)
    {
      return new Position((int)str [0] - ASCII, Int32.Parse(str.Substring(1)) - 1);
    }

  }

}