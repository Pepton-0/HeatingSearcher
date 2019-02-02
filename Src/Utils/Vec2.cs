using System;
using System.Linq;
using System.Collections.Generic;

public struct Vec2
{
    public static readonly Vec2 up = new Vec2(0, 1);
    public static readonly Vec2 left = new Vec2(-1, 0);
    public static readonly Vec2 down = new Vec2(0, -1);
    public static readonly Vec2 right = new Vec2(1, 0);

    /// <summary>
    /// Use for RandomDir()
    /// </summary>
    public static readonly Vec2[] dirs = new Vec2[] { up, left, down, right };

    public int x;
    public int y;

    public int SqrDistance
    {
        get
        {
            return x * x + y * y;
        }
    }

    public Vec2 (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString ()
    {
        return string.Concat("[", this.x, " : ", this.y, "]");
    }

    public static Vec2[] ShuffledDirs ()
    {
        return dirs.OrderBy(i => Guid.NewGuid()).ToArray();
    }

    public static bool operator == (Vec2 a, Vec2 b)
    {
        return a.x == b.x && a.y == b.y;
    }

    public static bool operator != (Vec2 a, Vec2 b)
    {
        return a.x != b.x || a.y != b.y;
    }

    public static Vec2 operator + (Vec2 a, Vec2 b)
    {
        return new Vec2(a.x + b.x, a.y + b.y);
    }

    public static Vec2 operator - (Vec2 a, Vec2 b)
    {
        return new Vec2(a.x - b.x, a.y - b.y);
    }

    public static Vec2 operator * (Vec2 a, int b)
    {
        return new Vec2(a.x * b, a.y * b);
    }

    public static Vec2 operator / (Vec2 a, int b)
    {
        return new Vec2(a.x / b, a.y / b);
    }

    public override bool Equals (object obj)
    {
        if (!(obj is Vec2))
        {
            return false;
        }

        var vec = (Vec2)obj;
        return x == vec.x &&
               y == vec.y;
    }

    public override int GetHashCode ()
    {
        var hashCode = 1362317449;
        hashCode = hashCode * -1521134295 + x.GetHashCode();
        hashCode = hashCode * -1521134295 + y.GetHashCode();
        hashCode = hashCode * -1521134295 + SqrDistance.GetHashCode();
        return hashCode;
    }
}