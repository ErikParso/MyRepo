using Kros.TroubleShooterClient.Model;
using System.Windows;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for MiniWindow.xaml
    /// </summary>
    public partial class MiniWindow : Window
    {
        private Patch patch;

        public Result ProblemFixed { get; private set; }

        public MiniWindow(Patch p)
        {
            InitializeComponent();
            chyba.Text = TroubleShooter.Current.RunData.ErrorMessage;
            Oprava.Text = p.Description;
            patch = p;
            ProblemFixed = Result.Canceled;
        }

        private void YesClick(object sender, RoutedEventArgs e)
        {
            ProblemFixed = patch.SolveProblemSafe() ? Result.Success : Result.Fail;
            this.Close();
        }

        private void NoClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        public enum Result
        {
            Success, Canceled, Fail
        }
    }


}
