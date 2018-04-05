using Kros.TroubleShooterClient.ViewModel;
using System.Windows;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for ComplexBrowserWindow.xaml
    /// </summary>
    public partial class ComplexBrowserWindow : Window
    {
        public bool Success { get; private set; }

        private PatchResultVM patchResult;

        public ComplexBrowserWindow(PatchResultVM patchResult)
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
                MessageBox.Show(
                    "Problém stále pretrváva. Ak máte problém s použitím inštrukcií, prosím kontaktujte podporu.");
            }
        }
    }
}
