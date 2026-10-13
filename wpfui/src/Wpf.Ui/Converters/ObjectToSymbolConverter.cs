using System;
using System.Windows.Data;
using Wpf.Ui.Common;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Converters;

internal class ObjectToSymbolConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is SymbolRegular symbol)
            return symbol;

        if (value is SymbolFilled symbolFilled)
            return symbolFilled.Swap();

        return SymbolRegular.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

