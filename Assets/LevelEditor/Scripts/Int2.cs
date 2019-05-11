using System;
using UnityEngine;

[System.Serializable]
public struct Int2 : System.IEquatable<Int2>
{
    public int x, y;

    public static readonly Int2 up = new Int2(0, 1);
    public static readonly Int2 down = new Int2(0, -1);
    public static readonly Int2 left = new Int2(-1, 0);
    public static readonly Int2 right = new Int2(1, 0);
    public static readonly Int2 zero = new Int2(0, 0);
    public static readonly Int2 one = new Int2(1, 1);
    public static readonly Int2 two = new Int2(2, 2);

    public Int2(int X, int Y)
    {
        x = X;
        y = Y;
    }

    public override string ToString()
    {

        return "x: " + x + " y: " + y;
    }
    public override int GetHashCode()
    {
        unchecked
        {
            return 7 + 3 * x + y << 15;
        }
    }

    public override bool Equals(object obj)
    {
        if (!(obj is Int2))
            return false;
        Int2 other = (Int2)obj;
        return x == other.x && y == other.y;
    }

    public static float Distance(Int2 a, Int2 b)
    {
        var x = a.x - b.x;
        var y = a.y - b.y;
        if (x < 0) x *= -1;
        if (y < 0) y *= -1;
        return x + y;
    }


    bool IEquatable<Int2>.Equals(Int2 other)
    {
        return (x == other.x && y == other.y);
    }

    public static implicit operator Vector2(Int2 i)
    {
        return new Vector2(i.x, i.y);
    }

    public static implicit operator Int2(Vector2 v)
    {
        return new Int2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }

    public static implicit operator Vector3(Int2 i)
    {
        return new Vector3(i.x, i.y, 0);
    }

    public static implicit operator Int2(Vector3 v)
    {
        return new Int2(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y));
    }
    public static bool operator ==(Int2 a, Int2 b)
    {
        return (a.x == b.x && a.y == b.y);
    }
    public static bool operator !=(Int2 a, Int2 b)
    {
        return !(a.x == b.x && a.y == b.y);
    }
    public static Int2 operator +(Int2 a, Int2 b)
    {
        a.x += b.x;
        a.y += b.y;
        return a;
    }
    public static Int2 operator *(Int2 a, Int2 b)
    {
        a.x *= b.x;
        a.y *= b.y;
        return a;
    }
    public static Int2 operator -(Int2 a, Int2 b)
    {
        a.x -= b.x;
        a.y -= b.y;
        return a;
    }
    public static Int2 operator -(Int2 a)
    {
        return new Int2(-a.x, -a.y);
    }
    public static Int2 operator *(Int2 a, int b)
    {
        return new Int2(a.x * b, a.y * b);
    }
    public static Int2 operator *(int b, Int2 a)
    {
        return new Int2(a.x * b, a.y * b);
    }
    public static Int2 operator /(Int2 a, int b)
    {
        return new Int2(a.x / b, a.y / b);
    }
    public Int2 normalized
    {
        get
        {
            return new Int2(x == 0 ? 0 : x / Mathf.Abs(x), y == 0 ? 0 : y / Mathf.Abs(y));
        }
    }
}