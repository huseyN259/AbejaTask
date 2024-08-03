using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WpfApp1.Converters;

public class StatusColorToForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var brush = value as SolidColorBrush;

        if (brush != null)
            if (brush.Color == Color.FromRgb(254, 255, 254))
                return Brushes.Black;

        return Brushes.White;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}

