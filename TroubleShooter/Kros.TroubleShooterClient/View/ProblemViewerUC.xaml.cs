using Kros.TroubleShooterClient.Model;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for ProblemViewerUC.xaml
    /// The root control of this app
    /// </summary>
    public partial class ProblemViewerUC : UserControl
    {
        /// <summary>
        /// initialise components and add actions to buttons
        /// </summary>
        public ProblemViewerUC()
        {
            InitializeComponent();
            //datacontext set
            DataContext = TroubleShooter.Current;
            //when select fix mode (question tree form) finds patches which can possibly repair problem 
            // -> display fix mode with these patches with no problem identification
            selectFixMode.PatchesSelected += patches => autoFixMode.Show(patches, true);
            // when autofix mode doesnt work, user can click continue to form mode to identify problem
            autoFixMode.RunFormMode += selectFixMode.Show;
            // when nothing else works, in form mode user can click to send data for servis
            selectFixMode.ServiceSelected += () => serviceMode.Show();
            // exit app on exit click on servis mode
            serviceMode.ExitClick += () => App.Current.MainWindow.Close();
            serviceMode.ProblemSent += () => serviceMode.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// find and repair problem fix
        /// </summary>
        public void detectProblem()
        {
            Task.Run(() =>
            {
                if (!autoFixMode.Show(TroubleShooter.Current.Patches))
                    if (MessageBox.Show(
                            "Nepodarilo sa identifikovať žiadne problémy vo vašom počítači. Prajete si zobraziť sprievodcu na vyhľadanie problému?",
                            "Troubleshooter", MessageBoxButton.YesNo, MessageBoxImage.Information) ==
                        MessageBoxResult.Yes)
                    {
                        selectFixMode.Dispatcher.Invoke(() => selectFixMode.Show());
                    }
            });
        }
    }
}
