using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Converts patch execution result (bool?) into patch color
    /// </summary>
    public class BoolColorConv : IValueConverter
    {
        /// <summary>
        /// Converts patch execution result (bool?) into patch color
        /// </summary>
        /// <param name="value">bool? patch result 
        ///     null - nonexecuted - blue
        ///     true - problem Fixed - green
        ///     false - problem not fixed - red
        /// </param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
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
