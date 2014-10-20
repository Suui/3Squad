using System;

namespace Medusa
{

  public struct Direction
  {

    public readonly int x;
    public readonly int y;
    public readonly int length;

    public static readonly Direction Left = new Direction(-1, 0);
    public static readonly Direction Right = Left * -1;
    public static readonly Direction Up = new Direction(0, 1);
    public static readonly Direction Down = Up * -1;

    public static readonly Direction[] All = {Up,Right,Down,Left};

    public Direction(int x, int y)
    {
      this.x = x;
      this.y = y;
      this.length = Math.Abs(x) + Math.Abs(y);
    }

    public static bool operator ==(Direction a, Direction b)
    {
      return a.x == b.x && a.x == b.x;
    }

    public static bool operator !=(Direction a, Direction b)
    {
      return a.x != b.x || a.y != b.y;
    }

    public override bool Equals(object obj)
    {
      if (obj == null)
        return false;
      return this == (Direction)obj;
    }

    public override int GetHashCode()
    {
      return x * 100 + y;
    }

    public static Direction operator +(Direction a, Direction b)
    {
      return new Direction(a.x + b.x, a.y + b.y);
    }

    public static Direction operator -(Direction a, Direction b)
    {
      return new Direction(a.x - b.x, a.y - b.y);
    }

    public static Direction operator *(Direction d, int s)
    {
      return new Direction(d.x * s, d.y * s);
    }

  }
}