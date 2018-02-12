using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for AutoFixModeUC.xaml
    /// </summary>
    public partial class AutoFixModeUC : UserControl
    {
        private AutoFixVm model;

        private Mode mode;

        public Action RunFormMode { get; set; }

        public AutoFixModeUC()
        {
            InitializeComponent();
            model = new AutoFixVm();
            DataContext = model;
            mode = Mode.SUMMARY;
            modeChange(this, null);
        }

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
                if (alreadyIdentified || patch.IdentifyProblemSafe())
                {
                    model.ProblemsFound++;
                    PatchResultVM patchResult = new PatchResultVM(patch, false);
                    Dispatcher.Invoke(() =>
                    {
                        model.PatchResults.Add(patchResult);
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

        private void FinishClick()
        {
            Visibility = Visibility.Hidden;
            Application.Current.Shutdown();
        }

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

        private void formModeClick()
        {
            this.Visibility = Visibility.Hidden;
            RunFormMode();
        }

        private void removePatchResult(object sender, MouseButtonEventArgs e)
        {
            PatchResultVM patch = ((Image)sender).DataContext as PatchResultVM;
            model.PatchResults.Remove(patch);
        }

        private void CloseHtmlClick()
        {
            HtmlInfo.Visibility = Visibility.Hidden;
        }

        private enum Mode
        {
            DETAIL, SUMMARY
        }

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

        private void displayHtmlDetail(object sender, MouseButtonEventArgs e)
        {
            string html = ((PatchResultVM)((TextBlock)sender).DataContext).HelpHtml;
            this.WebBrowser.NavigateToString(html);
            HtmlInfo.Visibility = Visibility.Visible;
        }
    }
}
