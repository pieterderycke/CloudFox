using System;
using System.Windows.Data;
using System.Globalization;
using CloudFox.Presentation.ViewModels;

namespace CloudFox.Presentation.Util
{
    public class EnumToIntegerConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Enum.ToObject(targetType, value);
        }
    }
}
