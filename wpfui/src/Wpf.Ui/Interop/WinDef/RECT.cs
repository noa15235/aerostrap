using System;
using System.Runtime.InteropServices;

namespace Wpf.Ui.Interop.WinDef;

[StructLayout(LayoutKind.Sequential)]
public struct RECT
{
    private int _left;
    private int _top;
    private int _right;
    private int _bottom;

    public int Left
    {
        get { return _left; }
        set { _left = value; }
    }

    public int Right
    {
        get { return _right; }
        set { _right = value; }
    }

    public int Top
    {
        get { return _top; }
        set { _top = value; }
    }

    public int Bottom
    {
        get { return _bottom; }
        set { _bottom = value; }
    }

    public int Width
    {
        get { return _right - _left; }
    }

    public int Height
    {
        get { return _bottom - _top; }
    }

    public POINT Position
    {
        get { return new POINT { x = _left, y = _top }; }
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

    public static RECT Union(RECT rect1, RECT rect2)
    {
        return new RECT
        {
            Left = Math.Min(rect1.Left, rect2.Left),
            Top = Math.Min(rect1.Top, rect2.Top),
            Right = Math.Max(rect1.Right, rect2.Right),
            Bottom = Math.Max(rect1.Bottom, rect2.Bottom),
        };
    }

    public override bool Equals(object obj)
    {
        if (obj is not RECT)
            return false;

        try
        {
            var rc = (RECT)obj;

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
