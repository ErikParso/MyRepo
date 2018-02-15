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
    /// </summary>
    public partial class ProblemViewerUC : UserControl
    {

        public ProblemViewerUC()
        {
            InitializeComponent();
            DataContext = TroubleShooter.Current;
            selectFixMode.PatchesSelected += patches => autoFixMode.Show(patches, true);
            autoFixMode.RunFormMode += selectFixMode.Show;
            selectFixMode.ServiceSelected += () => serviceMode.Show();
        }

        public void detectProblem()
        {
            Task.Run(() => autoFixMode.Show(TroubleShooter.Current.Patches));
        }
    }
}
