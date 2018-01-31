using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Kros.TroubleShooterClient.View
{
    public class BoolColorConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool? val = (bool?)value;
            if (val == null)
                return new SolidColorBrush(Colors.AliceBlue);
            else if (val == true)
                return new SolidColorBrush(Colors.LightGreen);
            else
                return new SolidColorBrush(Colors.LightPink);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
