using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace WpfApp1.Converters;

public class ZeroToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int intValue && intValue == 0)
			return Visibility.Collapsed;
		return Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
