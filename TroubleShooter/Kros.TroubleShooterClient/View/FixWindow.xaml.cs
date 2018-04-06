using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        /// if is displayed detail or summary
        /// </summary>
        private Mode mode;

        /// <summary>
        /// the question form mode click
        /// </summary>
        public Action RunFormMode { get; set; }

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
                        //identified at least 1 problem to return success
                        model.ProblemsFound++;
                        App.Current.Dispatcher.Invoke(() => model.PatchResults.Add(new PatchResultVM(patch)));
                    }
                    model.Progress.Add();
                }
                if (model.ProblemsFound == 0)
                {
                    model.CanRunForm = true;
                    model.CanExecute = false;
                }
                else
                {
                    model.Progress.Reset();
                    model.Progress.Count = model.PatchResults.Count(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED);
                    model.Progress.ActualWork = "Oprava problémov";
                }

                model.ButtonsEnabled = true;
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
                        model.ProblemsFixed++;
                        model.Progress.Add();
                    }
                }
                model.ButtonsEnabled = true;
                model.CanExecute = false;
                model.CanRunForm = true;
            });
        }

        /// <summary>
        /// Remove selected patch so it wont be executed.
        /// </summary>
        /// <param name="sender">the patch detail controll</param>
        /// <param name="e"></param>
        private void runSinglePatch(object sender, MouseButtonEventArgs e)
        {
            PatchResultVM patchVm = ((Image)sender).DataContext as PatchResultVM;
            patchVm.ExecutePatch();
            if (patchVm.ExecutionResult == ExecutionResult.FIXED)
            {
                model.ProblemsFixed++;
                model.Progress.Add();
            }
            //run all patches is unescessary if there is no patches to run O.o
            if (model.PatchResults.Count(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED) == 0)
            {
                model.CanExecute = false;
                model.CanRunForm = true;
            }
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
        /// Remove selected patch so it wont be executed.
        /// </summary>
        /// <param name="sender">the patch detail controll</param>
        /// <param name="e"></param>
        private void removePatchResult(object sender, MouseButtonEventArgs e)
        {
            PatchResultVM patch = ((Image)sender).DataContext as PatchResultVM;
            model.PatchResults.Remove(patch);
        }

        /// <summary>
        /// display mode
        /// </summary>
        private enum Mode
        {
            /// <summary>
            /// colored list of patches
            /// </summary>
            DETAIL,
            /// <summary>
            /// idetified and fixed counts
            /// </summary>
            SUMMARY
        }

        /// <summary>
        /// change 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void modeChange(object sender, MouseButtonEventArgs e)
        {
            if (mode == Mode.SUMMARY)
            {
                mode = Mode.DETAIL;
                Detail.Visibility = Visibility.Visible;
                modeButton.Opacity = 0.3;
                Summary.Visibility = Visibility.Hidden;
            }
            else
            {
                mode = Mode.SUMMARY;
                Detail.Visibility = Visibility.Hidden;
                modeButton.Opacity = 1;
                Summary.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// displays html detial for selected patch
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayHtmlDetail(object sender, MouseButtonEventArgs e)
        {
            PatchResultVM patch = (PatchResultVM)((Image)sender).DataContext;
            BrowserWindow browserWindow = new BrowserWindow(patch);
            browserWindow.ShowDialog();
            if (browserWindow.Success)
            {
                patch.ExecutionResult = ExecutionResult.FIXED;
                model.Progress.Add();
                model.ProblemsFixed++;
            }
        }
    }
}
