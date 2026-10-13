using System;
using System.Windows.Data;

namespace Wpf.Ui.Converters;

class ProgressThicknessConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
    {

        if (value is double height)
            return height / 8;

        return 12.0d;
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        System.Globalization.CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
