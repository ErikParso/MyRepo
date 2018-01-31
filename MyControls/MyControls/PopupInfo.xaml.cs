using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for PopupInfo.xaml
    /// </summary>
    public partial class PopupInfo : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register("MainText", typeof(string), typeof(PopupInfo), new UIPropertyMetadata(null));
        public static readonly DependencyProperty PlaceHolderProperty =
            DependencyProperty.Register("PlaceHolder", typeof(object), typeof(PopupInfo), new UIPropertyMetadata(null));
        public static readonly DependencyProperty ContentContextProperty =
            DependencyProperty.Register("ContentContext", typeof(object), typeof(PopupInfo), new UIPropertyMetadata(null));


        public event PropertyChangedEventHandler PropertyChanged;
        private void SetValueDP(DependencyProperty property, object value, [CallerMemberName]String p = null)
        {
            SetValue(property, value);
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(p));
        }

        public string MainText
        {
            get { return (string)GetValue(MainTextProperty); }
            set { SetValueDP(MainTextProperty, value); }
        }

        public object PlaceHolder
        {
            get { return (object)GetValue(PlaceHolderProperty); }
            set { SetValue(PlaceHolderProperty, value); }
        }

        public object ContentContext
        {
            get { return (object)GetValue(ContentContextProperty); }
            set { SetValue(ContentContextProperty, value); }
        }

        public PopupInfo()
        {
            InitializeComponent();
            //(this.Content as FrameworkElement).DataContext = this;
        }

        private void ImgArrowDown_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            displayDetail(true);
        }

        private void ImgArrowUp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            displayDetail(false);
        }

        private void displayDetail(bool shown)
        {
            if (shown)
            {
                ImgArrowDown.Visibility = Visibility.Hidden;
                ImgArrowUp.Visibility = Visibility.Visible;
                TbDetail.Visibility = Visibility.Visible;
            }
            else
            {
                ImgArrowDown.Visibility = Visibility.Visible;
                ImgArrowUp.Visibility = Visibility.Hidden;
                TbDetail.Visibility = Visibility.Collapsed;
            }
        }


    }
}
