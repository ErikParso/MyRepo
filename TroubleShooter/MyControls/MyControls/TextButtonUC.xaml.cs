﻿using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace MyControls
{
    /// <summary>
    /// Interaction logic for TextButtonUC.xaml
    /// </summary>
    public partial class TextButtonUC : UserControl, INotifyPropertyChanged
    {
        public static readonly DependencyProperty MainTextProperty =
            DependencyProperty.Register("MainText", typeof(string), typeof(TextButtonUC), new UIPropertyMetadata(null));
        public static readonly DependencyProperty DescriptionTexProperty =
            DependencyProperty.Register("DescriptionText", typeof(string), typeof(TextButtonUC), new UIPropertyMetadata(null));
        public static readonly DependencyProperty WithDescriptionProperty =
            DependencyProperty.Register("WithDescription", typeof(Visibility), typeof(TextButtonUC), new UIPropertyMetadata(Visibility.Collapsed));
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(TextButtonUC));
        public static readonly DependencyProperty BackColorProperty =
            DependencyProperty.Register("BackColor", typeof(Brush), typeof(TextButtonUC), 
                new UIPropertyMetadata(Brushes.WhiteSmoke));
        public static readonly DependencyProperty HoverColorProperty =
            DependencyProperty.Register("HoverColor", typeof(Brush), typeof(TextButtonUC),
                new UIPropertyMetadata(Brushes.Snow));
        public static readonly DependencyProperty ShadowColorProperty =
            DependencyProperty.Register("ShadowColor", typeof(Color), typeof(TextButtonUC), new UIPropertyMetadata(Colors.Black));

        public ImageSource Source
        {
            get { return (ImageSource)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

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

        public string DescriptionText
        {
            get { return (string)GetValue(DescriptionTexProperty); }
            set { SetValueDP(DescriptionTexProperty, value); }
        }

        public Visibility WithDescription
        {
            get { return (Visibility)GetValue(WithDescriptionProperty); }
            set { SetValueDP(WithDescriptionProperty, value); }
        }

        public Brush BackColor
        {
            get { return (Brush)GetValue(BackColorProperty); }
            set { SetValueDP(BackColorProperty, value); }
        }

        public Color ShadowColor
        {
            get { return (Color)GetValue(ShadowColorProperty); }
            set { SetValueDP(ShadowColorProperty, value); }
        }

        public Brush HoverColor
        {
            get { return (Brush)GetValue(HoverColorProperty); }
            set { SetValueDP(HoverColorProperty, value); }
        }

        public Action Click { get; set; }

        public TextButtonUC()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Click != null)
                Click();
        }
    }

    public class EnabledToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool)value)
                return new SolidColorBrush(Colors.Black);
            else
                return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

}
