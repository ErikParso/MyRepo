using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Kros.TroubleShooterClient.ViewModel;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Converts patch execution result (bool?) into patch color
    /// </summary>
    public class PatchColorConverter : IValueConverter
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
            switch ((ExecutionResult)value)
            {
                case ExecutionResult.NOT_EXECUTED:
                    return new SolidColorBrush(Colors.AliceBlue);
                case ExecutionResult.FIXED:
                    return new SolidColorBrush(Colors.LightGreen);
                case ExecutionResult.NOT_FIXED:
                    return new SolidColorBrush(Colors.LightPink);
                case ExecutionResult.INSTRUCTOR:
                    return new SolidColorBrush(Colors.LightPink);
                default:
                    return new SolidColorBrush(Colors.Transparent);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
