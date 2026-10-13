using System;
using Wpf.Ui.Common;

namespace Wpf.Ui.Extensions;

public static class SymbolExtensions
{
    public static SymbolFilled Swap(this SymbolRegular icon)
    {
        return Glyph.ParseFilled(icon.ToString());
    }

    public static SymbolRegular Swap(this SymbolFilled icon)
    {
        return Glyph.Parse(icon.ToString());
    }

    public static char GetGlyph(this SymbolRegular icon)
    {
        return ToChar(icon);
    }

    public static char GetGlyph(this SymbolFilled icon)
    {
        return ToChar(icon);
    }

    public static string GetString(this SymbolRegular icon)
    {
        return icon.GetGlyph().ToString();
    }

    public static string GetString(this SymbolFilled icon)
    {
        return icon.GetGlyph().ToString();
    }

    private static char ToChar(SymbolRegular icon)
    {
        return Convert.ToChar(icon);
    }

    private static char ToChar(SymbolFilled icon)
    {
        return Convert.ToChar(icon);
    }
}
