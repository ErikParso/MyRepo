using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.View;
using Kros.TroubleShooterClient.ViewModel;
using Kros.TroubleShooterInput;
using System;
using System.Text;
using System.Windows;

namespace Kros.TroubleShooterClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            switch (TroubleShooter.Current.RunData.StartupMode)
            {
                case StartupMode.COMPLEX: new MainWindow().Show(); break;
                case StartupMode.MINIMALISTIC: Environment.Exit(new MinimalisticMode().Run() ? 0 : 1); break;
                case StartupMode.SERVIS_ONLY: new ServisWindow().Show(); break;
            }
        }
    }
}
