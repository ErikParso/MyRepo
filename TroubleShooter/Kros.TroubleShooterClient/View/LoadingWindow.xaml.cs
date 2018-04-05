using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using Kros.TroubleShooterClient.Annotations;
using Kros.TroubleShooterClient.Model;

namespace Kros.TroubleShooterClient.View
{
    /// <summary>
    /// Interaction logic for LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window, INotifyPropertyChanged
    {
        private string actualWork = "Spúšťam TroubleShooter...";

        public string ActualWork { get => actualWork;
            set { actualWork = value;
                OnPropertyChanged();
            }
        }

        public LoadingWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
