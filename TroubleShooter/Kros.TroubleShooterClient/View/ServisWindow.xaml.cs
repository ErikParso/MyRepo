using System.Windows;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for ServisWindow.xaml
    /// </summary>
    public partial class ServisWindow : Window
    {
        public ServisWindow()
        {
            InitializeComponent();
            servis.ExitClick += () => this.Close();
            servis.ProblemSent += () => this.Close();
        }
    }
}
