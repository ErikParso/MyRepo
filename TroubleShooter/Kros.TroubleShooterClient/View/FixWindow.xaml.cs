using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for FixWindow.xaml
    /// </summary>
    public partial class FixWindow : Window
    {
        /// <summary>
        /// this controls model
        /// </summary>
        private AutoFixVm model;

        /// <summary>
        /// initialises controller
        /// </summary>
        public FixWindow()
        {
            InitializeComponent();
            model = new AutoFixVm();
            DataContext = model;
            SpProgressContext.DataContext = model.Progress;
        }

        /// <summary>
        /// Resets and displays this controller. Identifies problem using <see cref="Patch.ComplexIdentifySafe"/> method.
        /// If alreadyIdentified param is set true as it is done in form mode, all passed patches will be displayed for execution.
        /// </summary>
        /// <param name="patches">available patches</param>
        /// <param name="alreadyIdentified">if problems are already identified - used from form mode</param>
        /// <returns>if no problem was identified return false</returns>
        public void Run(IEnumerable<Patch> patches, bool alreadyIdentified = false)
        {
            model.Reset();
            model.ButtonsEnabled = false;
            model.CanExecute = true;
            model.CanRunForm = false;
            model.Progress.ActualWork = "Identifikácia problémov";
            model.Progress.Count = patches.Count();

            Task.Run(() =>
            {
                foreach (Patch patch in patches)
                {
                    if (alreadyIdentified || patch.ComplexIdentifySafe() || patch.FastIdentifySafe())
                    {
                        App.Current.Dispatcher.Invoke(() => model.PatchResults.Add(new PatchResultVM(patch)));
                        model.HasProblems = true;
                    }
                    model.Progress.Add();
                }
                if (model.PatchResults.Any())
                {
                    model.Progress.Reset();
                    model.Progress.Count = model.PatchResults.Count(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED);
                    model.Progress.ActualWork = "Oprava problémov";
                }
                else
                {
                    model.CanRunForm = true;
                    model.CanExecute = false;
                }
                model.ButtonsEnabled = true;
                model.RefreshCheck();
            });
        }

        /// <summary>
        /// Close troubleshooter on finish click
        /// </summary>
        private void FinishClick()
        {
            Visibility = Visibility.Hidden;
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// Executes all patches which was selectet in process <see cref="Run(IEnumerable{Patch}, bool)"/>.
        /// </summary>
        private void ExecutePatchesClick()
        {
            model.ButtonsEnabled = false;
            Task.Run(() =>
            {
                foreach (PatchResultVM patchVm in model.PatchResults.Where(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED))
                {
                    patchVm.ExecutePatch();
                    if (patchVm.ExecutionResult == ExecutionResult.FIXED)
                    {
                        model.Progress.Add();
                    }
                }
                if (!model.PatchResults.Where(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED).Any())
                {
                    model.CanExecute = false;
                    model.CanRunForm = true;
                }
                model.ButtonsEnabled = true;
                model.RefreshCheck();
            });
        }

        /// <summary>
        /// reset and hide this control and fire run form control action
        /// </summary>
        private void formModeClick()
        {
            new QuestionWindow().Show();
            Close();
        }

        /// <summary>
        /// displays html detial for selected patch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayHtmlDetail(object sender, RoutedEventArgs e)
        {
            PatchResultVM patch = (PatchResultVM)((Button)sender).DataContext;
            BrowserWindow browserWindow = new BrowserWindow(patch);
            browserWindow.ShowDialog();
            if (browserWindow.Success)
            {
                patch.ExecutionResult = ExecutionResult.FIXED;
                model.Progress.Add();
            }
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            model.RefreshCheck();
        }
    }
}
