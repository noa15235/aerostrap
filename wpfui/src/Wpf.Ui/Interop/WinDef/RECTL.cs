using System;
using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

[StructLayout(LayoutKind.Sequential)]
public struct RECTL
{
    private long _left;
    private long _top;
    private long _right;
    private long _bottom;

    public long Left
    {
        get { return _left; }
        set { _left = value; }
    }

    public long Right
    {
        get { return _right; }
        set { _right = value; }
    }

    public long Top
    {
        get { return _top; }
        set { _top = value; }
    }

    public long Bottom
    {
        get { return _bottom; }
        set { _bottom = value; }
    }

    public long Width
    {
        get { return _right - _left; }
    }

    public long Height
    {
        get { return _bottom - _top; }
    }

    public POINTL Position
    {
        get { return new POINTL { x = _left, y = _top }; }
    }

    public SIZE Size
    {
        get { return new SIZE { cx = Width, cy = Height }; }
    }

    public void Offset(int dx, int dy)
    {
        _left += dx;
        _top += dy;
        _right += dx;
        _bottom += dy;
    }

    public static RECTL Union(RECTL rect1, RECTL rect2)
    {
        return new RECTL
        {
            Left = Math.Min(rect1.Left, rect2.Left),
            Top = Math.Min(rect1.Top, rect2.Top),
            Right = Math.Max(rect1.Right, rect2.Right),
            Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
        };
    }

    public override bool Equals(object obj)
    {
        if (obj is not RECTL)
            return false;

        try
        {
            var rc = (RECTL)obj;
            return rc._bottom == _bottom
                && rc._left == _left
                && rc._right == _right
                && rc._top == _top;
        }
        catch (InvalidCastException)
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return _top.GetHashCode() ^ _bottom.GetHashCode() ^ _left.GetHashCode() ^ _right.GetHashCode();
    }
}
