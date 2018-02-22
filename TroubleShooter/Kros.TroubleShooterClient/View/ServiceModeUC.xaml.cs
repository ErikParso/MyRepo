using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.Service;
using Kros.TroubleShooterClient.ViewModel;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for ServiceModeUC.xaml
    /// Servis mode can send data specified in model <see cref="ServiceModeVM"/>
    /// </summary>
    public partial class ServiceModeUC : UserControl
    {
        /// <summary>
        /// datacontext of this controll
        /// </summary>
        private ServiceModeVM model;

        /// <summary>
        /// Event fired on exit button clic
        /// </summary>
        public Action ExitClick;

        /// <summary>
        /// When problem is successfully sent to servis
        /// </summary>
        public Action ProblemSent;

        /// <summary>
        /// sending to server failed
        /// </summary>
        public Action ProlemSentFailed;

        /// <summary>
        /// initiqalise components and set datacontext
        /// </summary>
        public ServiceModeUC()
        {
            InitializeComponent();
            model = new ServiceModeVM();
            DataContext = model;
            if (TroubleShooter.Current.RunData?.Attachments != null)
                foreach (string defAtt in TroubleShooter.Current.RunData.Attachments)
                    if (File.Exists(defAtt))
                        attachments.Files.Add(defAtt);
        }

        /// <summary>
        /// displays this control
        /// </summary>
        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// tries to send data to to servis
        /// </summary>
        private void SendClick()
        {
            ServisManager m = new ServisManager();
            bool result = m.SendToServis(attachments.Files, model.Properties);
            if (result == true)
            {
                MessageBox.Show("Problém bol úspešne odoslaný na náš server.", "Servis", MessageBoxButton.OK, icon: MessageBoxImage.Information);
                if (ProblemSent != null)
                    ProblemSent();
            }
            else
            {
                MessageBox.Show("Problém sa nepodarilo odoslať. Server práve nie je dostupný alebo máte problém s pripojením na internet.", "Servis", MessageBoxButton.OK, icon: MessageBoxImage.Error);
                if (ProlemSentFailed != null)
                    ProlemSentFailed();
            }
        }

        /// <summary>
        /// attachment file explorer icon click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileExplorerClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Multiselect = true;
            if (d.ShowDialog() == true)
            {
                foreach (string filename in d.FileNames)
                    if (!attachments.Files.Contains(filename))
                        attachments.Files.Add(filename);
            }
        }

        /// <summary>
        /// servis data properties marked as path has a file explorer.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectPath(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == true)
            {
                ((OptionalServiceProp)((Image)sender).DataContext).Value = d.FileName;
            }
        }

        private void FinishClick()
        {
            if (ExitClick != null)
                ExitClick();
        }
    }
}
