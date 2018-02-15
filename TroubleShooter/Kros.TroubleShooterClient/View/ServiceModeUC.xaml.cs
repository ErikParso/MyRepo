using Kros.TroubleShooterClient.Service;
using Kros.TroubleShooterClient.ViewModel;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for ServiceModeUC.xaml
    /// </summary>
    public partial class ServiceModeUC : UserControl
    {
        private ServiceModeVM model; 

        public ServiceModeUC()
        {
            InitializeComponent();
            model = new ServiceModeVM();
            model.DefineProperties();
            DataContext = model;
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        private void DropFile(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            }
        }

        private void SendClick()
        {
            ServiceManager m = new ServiceManager();
            bool result = m.SendToServis(attachments.Files, model.Properties);           
        }

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

        private void SelectPath(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            if (d.ShowDialog() == true)
            {
                ((OptionalServiceProp)((Image)sender).DataContext).Value = d.FileName;
            }
        }
    }
}
