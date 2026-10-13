using System;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

internal class IconNotEmptyConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is Common.SymbolRegular icon)
            return icon != Common.SymbolRegular.Empty;

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
