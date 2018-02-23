using System;
using System.Globalization;
using System.Windows.Data;

namespace MyControls
{
    public class EnabledToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return 1;
            else
                return 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
