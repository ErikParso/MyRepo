using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.ViewModel;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for SelectFixModeUC.xaml
    /// </summary>
    public partial class SelectFixModeUC : UserControl
    {
        private QuestionMode model;

        public Action<IEnumerable<Patch>> PatchesSelected; 

        public SelectFixModeUC()
        {
            InitializeComponent();
            model = new QuestionMode(TroubleShooter.Current.RootQuestion);
            this.DataContext = model;
        }

        public void Show()
        {
            model.Reset();
            GuiFuncs.SetVisibility(this, Visibility.Visible);
        }

        private void FinishClick()
        {
            this.Visibility = Visibility.Hidden;
            Application.Current.Shutdown();
        }

        private void ProblemSelected(object sender, MouseButtonEventArgs e)
        {
            model.SelectAnswer((QuestionMode.Answer)((UserControl)sender).DataContext);
            //user answered a last question a nd we can execute patches now
            if (model.Answers.Count == 0)
            {
                this.Visibility = Visibility.Hidden;
                PatchesSelected(model.Patches);
            }
        }

        private void ProblemLinkClick(object sender, MouseButtonEventArgs e)
        {
            model.SelectBack((Question)((UserControl)sender).DataContext);
        }
    }
}
