using System;

namespace Wpf.Ui.Common;

public static class Glyph
{
    public const SymbolRegular DefaultIcon = SymbolRegular.Heart28;

    public const SymbolFilled DefaultFilledIcon = SymbolFilled.Heart28;

    public static Common.SymbolRegular Parse(string name)
    {
        if (String.IsNullOrEmpty(name))
            return DefaultIcon;

        return (Common.SymbolRegular)Enum.Parse(typeof(Common.SymbolRegular), name);
    }

    public static Common.SymbolFilled ParseFilled(string name)
    {
        if (String.IsNullOrEmpty(name))
            return DefaultFilledIcon;

        return (Common.SymbolFilled)Enum.Parse(typeof(Common.SymbolFilled), name);
    }
}
