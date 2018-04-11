using Kros.TroubleShooterClient.ViewModel;
using System.Windows;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for BrowserWindow.xaml
    /// </summary>
    public partial class BrowserWindow : Window
    {
        public bool Success { get; private set; }

        private PatchResultVM patchResult;

        public BrowserWindow(PatchResultVM patchResult)
        {
            InitializeComponent();
            this.patchResult = patchResult;
            this.WebBrowser.NavigateToString(patchResult.HelpHtml);
        }

        /// <summary>
        /// hides patch detail
        /// </summary>
        private void CloseHtmlClick()
        {
            Close();
        }

        private void MarkSolvedClick()
        {
            if (patchResult.InstructionsResult())
            {
                Success = true;
                Close();
            }
            else
            {
                MessageBox.Show("Problém naďalej pretrváva. Ak máte problém s použitím inštrukcií, prosím kontaktujte oddelenie podpory.", 
                    "Troubleshooter", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// hides patch detail
        /// </summary>
        private void IgnoreClick()
        {
            Close();
        }
    }
}
