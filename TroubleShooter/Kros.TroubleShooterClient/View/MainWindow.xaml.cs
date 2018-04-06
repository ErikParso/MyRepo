using Kros.TroubleShooterClient.Model;
using System.Windows;
using Kros.TroubleShooterClient.View;
using System.Threading.Tasks;

namespace Kros.TroubleShooterClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// initialises components
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        public void detectProblem()
        {
            FixWindow fixWindow = new FixWindow();
            fixWindow.Show();
            Close();
            fixWindow.Run(TroubleShooter.Current.Patches);
        }
    }
}
