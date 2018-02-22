using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for AutoFixModeUC.xaml
    /// </summary>
    public partial class AutoFixModeUC : UserControl
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
        public AutoFixModeUC()
        {
            InitializeComponent();
            model = new AutoFixVm();
            DataContext = model;
            mode = Mode.SUMMARY;
            //this display detail 
            modeChange(this, null);
        }

        /// <summary>
        /// Resets and displays this controller. Identifies problem using <see cref="Patch.ComplexIdentifySafe"/> method.
        /// If alreadyIdentified param is set true as it is done in form mode, all passed patches will be displayed for execution.
        /// </summary>
        /// <param name="patches">available patches</param>
        /// <param name="alreadyIdentified">if problems are already identified - used from form mode</param>
        public void Show(IEnumerable<Patch> patches, bool alreadyIdentified = false)
        {
            GuiFuncs.SetVisibility(this, Visibility.Visible);
            GuiFuncs.SetEnabled(executePatchesButton, false);
            GuiFuncs.SetVisibility(executePatchesButton, Visibility.Visible);
            GuiFuncs.SetEnabled(finishButton, false);
            GuiFuncs.SetVisibility(formModeButton, Visibility.Hidden);
            GuiFuncs.ChangeDataContext(SpProgressContext, model.DetectProblemsProgress);
            model.Reset();
            model.DetectProblemsProgress.ActualWork = "Identifikácia problémov";
            model.DetectProblemsProgress.Count = patches.Count();
            foreach (Patch patch in patches)
            {
                if (alreadyIdentified || patch.ComplexIdentifySafe() || patch.FastIdentifySafe())
                {
                    model.ProblemsFound++;
                    Dispatcher.Invoke(() =>
                    {
                        model.PatchResults.Add(new PatchResultVM(patch));
                    });
                }
                model.DetectProblemsProgress.Add();
            }
            if (model.ProblemsFound == 0)
            {
                GuiFuncs.SetVisibility(formModeButton, Visibility.Visible);
                GuiFuncs.SetVisibility(executePatchesButton, Visibility.Hidden);
            }
            model.DetectProblemsProgress.Set100();
            GuiFuncs.SetEnabled(finishButton, true);
            GuiFuncs.SetEnabled(executePatchesButton, true);
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
        /// Executes all patches which was selectet in process <see cref="Show(IEnumerable{Patch}, bool)"/>.
        /// </summary>
        private void ExecutePatchesClick()
        {
            GuiFuncs.SetEnabled(finishButton, false);
            GuiFuncs.SetEnabled(executePatchesButton, false);
            GuiFuncs.ChangeDataContext(SpProgressContext, model.SolveProblemsProgress);
            model.SolveProblemsProgress.Reset();
            model.SolveProblemsProgress.ActualWork = "Vykonávam opravu";
            model.SolveProblemsProgress.Count = model.PatchResults.Where(p => p.ProblemFixed == null).Count();
            new Thread(() =>
            {
                foreach (PatchResultVM patchVm in model.PatchResults.Where(p => p.ProblemFixed == null))
                {
                    patchVm.ExecutePatch();
                    if (patchVm.ProblemFixed == true)
                        model.ProblemsFixed++;
                    model.SolveProblemsProgress.Add();
                }
                GuiFuncs.SetEnabled(finishButton, true);
                model.SolveProblemsProgress.Set100();
                GuiFuncs.SetVisibility(executePatchesButton, Visibility.Hidden);
                GuiFuncs.SetVisibility(formModeButton, Visibility.Visible);
            }).Start();
        }

        /// <summary>
        /// reset and hide this control and fire run form control action
        /// </summary>
        private void formModeClick()
        {
            model.Reset();
            this.Visibility = Visibility.Hidden;
            RunFormMode();
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
        /// hides patch detail
        /// </summary>
        private void CloseHtmlClick()
        {
            HtmlInfo.Visibility = Visibility.Hidden;
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
            string html = ((PatchResultVM)((TextBlock)sender).DataContext).HelpHtml;
            this.WebBrowser.NavigateToString(html);
            HtmlInfo.Visibility = Visibility.Visible;
        }
    }
}
