using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf.Ui.Converters;

internal class BrushToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
            return brush.Color;

        if (value is Color)
            return value;

        return new Color { A = 255, R = 255, G = 0, B = 0 };
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
