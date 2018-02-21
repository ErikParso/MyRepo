using System.Windows;
using System.Windows.Controls;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Provides functionalities over controls where dispatcher is needed
    /// </summary>
    public class GuiFuncs
    {
        /// <summary>
        /// Disables control using its dispatcher
        /// </summary>
        /// <param name="c">control to be set</param>
        public static void SetEnabled(Control c, bool state)
        {
            c.Dispatcher.Invoke(() => c.IsEnabled = state);
        }

        /// <summary>
        /// Sets visibility of controll using its dispatcher
        /// </summary>
        /// <param name="c"></param>
        /// <param name="visibility"></param>
        public static void SetVisibility(Control c, Visibility visibility)
        {
            c.Dispatcher.Invoke(() => c.Visibility = visibility);
        }

        /// <summary>
        /// Changes datacontext of controll in its dispatcher. Returns the old one.
        /// </summary>
        /// <param name="c">controll</param>
        /// <param name="newDC">new datacontext</param>
        /// <returns>old data context</returns>
        public static object ChangeDataContext(FrameworkElement c, object newDC)
        {
            object oldDc = null;
            c.Dispatcher.Invoke(() =>
            {
                oldDc = c.DataContext;
                c.DataContext = newDC;
            });
            return oldDc;
        }
    }
}
