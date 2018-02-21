using Kros.TroubleShooterClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        }

        /// <summary>
        /// find and repair problem fix
        /// </summary>
        public void detectProblem()
        {
            Task.Run(() => autoFixMode.Show(TroubleShooter.Current.Patches));
        }
    }
}
