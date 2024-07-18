using System.Diagnostics;

namespace Sapia.Game.Hack.Structs;

[DebuggerDisplay("{X} / {Y}")]
public readonly struct Coord : IComparable<Coord>, IEquatable<Coord>
{
    public int X { get; }
    public int Y { get; }

    public Coord(int x, int y)
    {
        X = x;
        Y = y;
    }

    public Coord((int x, int y) c)
    {
        X = c.x;
        Y = c.y;
    }


    public static implicit operator (int x, int y)(Coord c) => (c.X, c.Y);
    public static implicit operator Coord((int X, int Y) c) => new(c.X, c.Y);

    public void Deconstruct(out int x, out int y)
    {
        x = X;
        y = Y;
    }

    public override bool Equals(object? obj)
    {
        return obj is Coord other && Equals(other);
    }

    public bool Equals(Coord other)
    {
        return X == other.X && Y == other.Y;
    }

    public override int GetHashCode()
    {
        unchecked // Overflow is fine, just wrap
        {
            int hash = 17;
            hash = hash * 23 + X;
            hash = hash * 23 + Y;
            return hash;
        }
    }

    public static Coord operator +(Coord a, Coord b)
    {
        return new Coord(a.X + b.X, a.Y + b.Y);
    }

    public static bool operator ==(Coord a, Coord b)
    {
        return a.Equals(b);
    }

    public static bool operator !=(Coord coordinate1, Coord coordinate2)
    {
        return !(coordinate1 == coordinate2);
    }

    public static bool operator ==(Coord coordinate1, (int x, int y) coordinate2)
    {
        return coordinate1.X == coordinate2.x && coordinate1.Y == coordinate2.y;
    }

    public static bool operator !=(Coord coordinate1, (int x, int y) coordinate2)
    {
        return !(coordinate1 == coordinate2);
    }

    public static Coord Zero() => new(0, 0);

    public int CompareTo(Coord other)
    {
        var xComparison = X.CompareTo(other.X);
        if (xComparison != 0) return xComparison;
        return Y.CompareTo(other.Y);
    }

    public override string ToString() => $"{X} / {Y}";

    public static int Distance(Coord a, Coord b)
    {
        var dx = b.X - a.X;
        var dy = b.Y - a.Y;

        var df = (dx * dx) + (dy * dy);

        return (int)Math.Ceiling(Math.Sqrt(df));
    }
}