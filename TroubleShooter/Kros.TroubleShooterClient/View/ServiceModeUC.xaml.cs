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

        private void SendClick()
        {
            ServisManager m = new ServisManager();
            bool result = m.SendToServis(attachments.Files, model.Properties);
            if (result == true)
                MessageBox.Show("Problém bol úspešne odoslaný na náš server.", "Servis", MessageBoxButton.OK , icon: MessageBoxImage.Information);      
            else
                MessageBox.Show("Problém sa nepodarilo odoslať. Server práve nie je dostupný alebo máte problém s pripojením na internet.", "Servis", MessageBoxButton.OK, icon: MessageBoxImage.Error);
            this.Visibility = Visibility.Hidden;
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
