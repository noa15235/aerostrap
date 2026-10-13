using System;
using System.Windows.Data;
using Wpf.Ui.Common;
using Wpf.Ui.Extensions;

namespace Wpf.Ui.Converters;

internal class IconToStringConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value == null)
            return null;

        if (value is string)
            return value;

        if (value is char c)
            return c.ToString();

        if (value is SymbolRegular icon)
            return icon.GetString();

        if (value is SymbolFilled iconFilled)
            return iconFilled.GetString();

        return null;
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
