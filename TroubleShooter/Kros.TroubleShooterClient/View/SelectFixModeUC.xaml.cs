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
    /// The form mode (question tree for identify problem solving patches)
    /// </summary>
    public partial class SelectFixModeUC : UserControl
    {
        /// <summary>
        /// model toi this control
        /// </summary>
        private QuestionMode model;

        /// <summary>
        /// When user navigates to answer which indicates problem this event is fired
        /// </summary>
        public Action<IEnumerable<Patch>> PatchesSelected;

        /// <summary>
        /// when user clicks to send data for servis this action will be executed
        /// </summary>
        public Action ServiceSelected;

        /// <summary>
        /// initialises components, set datacontext, enable servis button if server is online
        /// </summary>
        public SelectFixModeUC()
        {
            InitializeComponent();
            model = new QuestionMode(TroubleShooter.Current.RootQuestion);
            this.DataContext = model;
            ServiceButton.IsEnabled = TroubleShooter.Current.ServerOnline;
        }

        /// <summary>
        /// displays this control
        /// </summary>
        public void Show()
        {
            model.Reset();
            GuiFuncs.SetVisibility(this, Visibility.Visible);
        }

        /// <summary>
        /// exit troubleshooter click
        /// </summary>
        private void FinishClick()
        {
            this.Visibility = Visibility.Hidden;
            Application.Current.MainWindow.Close();
        }

        /// <summary>
        /// run servis mode click
        /// </summary>
        private void ServiceClick()
        {
            this.Visibility = Visibility.Hidden;
            ServiceSelected();
        }

        /// <summary>
        /// Users click to answer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProblemSelected(object sender, MouseButtonEventArgs e)
        {
            //select answer in model - this will actualise a new question
            model.SelectAnswer((QuestionMode.Answer)((UserControl)sender).DataContext);
            //if there are no answers it is a final question and corresponded patches will be executed
            if (model.Answers.Count == 0)
            {
                this.Visibility = Visibility.Hidden;
                PatchesSelected(model.Patches);
            }
        }

        /// <summary>
        /// click on the element of a question link will navigate you back in specific question
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProblemLinkClick(object sender, MouseButtonEventArgs e)
        {
            model.SelectBack((Question)((UserControl)sender).DataContext);
        }
    }
}
