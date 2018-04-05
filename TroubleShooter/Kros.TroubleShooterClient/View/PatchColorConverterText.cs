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
    public class PatchColorConverterText : IValueConverter
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
            if (targetType == typeof(string))
            {
                switch ((ExecutionResult)value)
                {
                    case ExecutionResult.NOT_EXECUTED:
                        return "Oprava ešte neprebehla. Pre spustenie kliknite na tlačidlo oprav.";
                    case ExecutionResult.FIXED:
                        return "Zdá sa, že oprava chyby prebehla úspešne.";
                    case ExecutionResult.NOT_FIXED:
                        return "Problém sa nepodarilo opraviť.";
                    case ExecutionResult.INSTRUCTOR:
                        return "Troubleshooter nedokáže tento problém vyriešiť ale poskytne návod na jeho odstránenie.";
                    default:
                        return "...";
                }
            }
            else
            {
                switch ((ExecutionResult)value)
                {
                    case ExecutionResult.NOT_EXECUTED:
                        return new SolidColorBrush(Colors.DarkBlue);
                    case ExecutionResult.FIXED:
                        return new SolidColorBrush(Colors.DarkGreen);
                    case ExecutionResult.NOT_FIXED:
                        return new SolidColorBrush(Colors.DarkRed);
                    case ExecutionResult.INSTRUCTOR:
                        return new SolidColorBrush(Colors.DarkRed);
                    default:
                        return new SolidColorBrush(Colors.Black);
                }
            }


        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
