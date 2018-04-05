using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// model for auto fix control
    /// </summary>
    public class AutoFixVm : ObservableObject
    {
        /// <summary>
        /// displayed patches - patches which idetified problem
        /// </summary>
        public ObservableCollection<PatchResultVM> PatchResults { get; set; }

        /// <summary>
        /// inner progres model - shows problem identification
        /// </summary>
        public ProgressVM Progress { get; private set; }

        /// <summary>
        /// number of found problems
        /// </summary>
        private int _problemsFound;
        /// <summary>
        /// number of found problems
        /// </summary>
        public int ProblemsFound
        {
            get { return _problemsFound; }
            set { _problemsFound = value; RaisePropertyChanged("ProblemsFound"); }
        }

        private bool _canExecute;
        public bool CanExecute
        {
            get { return _canExecute; }
            set { _canExecute = value; RaisePropertyChanged("CanExecute"); }
        }

        private bool _canRunForm;
        public bool CanRunForm
        {
            get { return _canRunForm; }
            set { _canRunForm = value; RaisePropertyChanged("CanRunForm"); }
        }

        private bool _buttonsEnabled;
        public bool ButtonsEnabled
        {
            get { return _buttonsEnabled; }
            set { _buttonsEnabled = value; RaisePropertyChanged("ButtonsEnabled"); }
        }

        /// <summary>
        /// number of fixed problems
        /// </summary>
        private int _problemsFixed;
        /// <summary>
        /// number of fixed problems
        /// </summary>
        public int ProblemsFixed
        {
            get { return _problemsFixed; }
            set { _problemsFixed = value; RaisePropertyChanged("ProblemsFixed"); }
        }

        /// <summary>
        /// init this model and progress models
        /// </summary>
        public AutoFixVm()
        {
            PatchResults = new ObservableCollection<PatchResultVM>();
            Progress = new ProgressVM();
        }

        /// <summary>
        /// reset this model
        /// </summary>
        internal void Reset()
        {
            Progress.Reset();
            ProblemsFound = 0;
            ProblemsFixed = 0;
            App.Current.Dispatcher.Invoke(() => PatchResults.Clear());
        }
    }

    /// <summary>
    /// displays decimal as a percentage vale - used in xaml as value converter
    /// </summary>
    public class PercentageConverter : IValueConverter
    {
        /// <summary>
        /// converts double to percentage
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double val = (double)value;
            return string.Format("{0}%", (int)(val * 100));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
