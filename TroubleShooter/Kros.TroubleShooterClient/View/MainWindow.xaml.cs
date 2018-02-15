using Kros.TroubleShooterClient.Model;
using System.Windows;


namespace Kros.TroubleShooterClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(TroubleShooter troubleShooter)
        {
            InitializeComponent();
            this.DataContext = troubleShooter;
        }
    }
}
