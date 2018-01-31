using Kros.TroubleShooterClient.Model;
using System.ServiceModel;
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
            TroubleShooter.Current.Error = @"Pri behu programu sa vyskytla neočakávaná chyba 1 - 1009 - X210 - 5\n" + Properties.Resources.lorem;
            InitializeComponent();
        }
    }
}
