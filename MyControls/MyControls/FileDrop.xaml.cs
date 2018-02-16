using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
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

namespace MyControls
{
    /// <summary>
    /// Interaction logic for FileDrop.xaml
    /// </summary>
    public partial class FileDrop : UserControl
    {
        public ObservableCollection<string> Files { get; set; }

        public FileDrop()
        {
            InitializeComponent();
            Files = new ObservableCollection<string>();
            DataContext = this;
        }

        private void UniformGrid_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Note that you can have more than one file.
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string file in files)
                    if (!Files.Contains(file) && File.Exists(file))
                        Files.Add(file);
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Files.Remove((string)((Image)sender).DataContext);
        }
    }

    public class FileNameValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return System.IO.Path.GetFileName((string)value);
           
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
