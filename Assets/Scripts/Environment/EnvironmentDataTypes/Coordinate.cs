using UnityEngine;

// Replacement for Vector2Int, which was causing slowdowns in big loops due to x,y accessor overhead
[System.Serializable]
public struct Coordinate
{

    public int x;
    public int y;

    public Coordinate(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static int GetSquaredDistance(Coordinate a, Coordinate b)
    {
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y);
    }

    public static float Distance(Coordinate a, Coordinate b)
    {
        return (float)System.Math.Sqrt((a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y));
    }

    public static bool AreNeighbourCoordinates(Coordinate a, Coordinate b)
    {
        return System.Math.Abs(a.x - b.x) <= 1 && System.Math.Abs(a.y - b.y) <= 1;
    }

    public static Coordinate invalid
    {
        get
        {
            return new Coordinate(-1, -1);
        }
    }

    public static Coordinate up
    {
        get
        {
            return new Coordinate(0, 1);
        }
    }

    public static Coordinate down
    {
        get
        {
            return new Coordinate(0, -1);
        }
    }

    public static Coordinate left
    {
        get
        {
            return new Coordinate(-1, 0);
        }
    }

    public static Coordinate right
    {
        get
        {
            return new Coordinate(1, 0);
        }
    }

    public static Coordinate operator +(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.x + b.x, a.y + b.y);
    }

    public static Coordinate operator -(Coordinate a, Coordinate b)
    {
        return new Coordinate(a.x - b.x, a.y - b.y);
    }

    public static bool operator ==(Coordinate a, Coordinate b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator !=(Coordinate a, Coordinate b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static implicit operator Vector2(Coordinate v)
    {
        return new Vector2(v.x, v.y);
    }

    public static implicit operator Vector3(Coordinate v)
    {
        return new Vector3(v.x, 0, v.y);
    }

    public override bool Equals(object other)
    {
        return (Coordinate)other == this;
    }

    public override int GetHashCode()
    {
        return 0;
    }

    public override string ToString()
    {
        return "(" + x + " ; " + y + ")";
    }
}