using System.Windows;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window
    {
        public BrowserWindow(string html)
        {
            InitializeComponent();
            this.WebBrowser.NavigateToString(html);
        }

        private void FinishClick()
        {
            Close();
        }
    }
}
