using System;
using System.Windows.Data;
using System.Windows.Media;

namespace Wpf.Ui.Converters;

internal class FallbackBrushConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        if (value is SolidColorBrush brush)
            return brush;

        if (value is Color)
            return new SolidColorBrush((Color)value);

        return new SolidColorBrush(new Color { A = 255, R = 255, G = 0, B = 0 });
    }

    public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
