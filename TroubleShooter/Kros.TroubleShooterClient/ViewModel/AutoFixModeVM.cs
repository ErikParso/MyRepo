using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Kros.TroubleShooterClient.ViewModel
{
    public class AutoFixVm : ObservableObject
    {
        public ObservableCollection<PatchResultVM> PatchResults { get; set; }

        public ProgressVM DetectProblemsProgress { get; private set; }

        public ProgressVM SolveProblemsProgress { get; private set; }

        private int _problemsFound;
        public int ProblemsFound
        {
            get { return _problemsFound; }
            set { _problemsFound = value; RaisePropertyChanged("ProblemsFound"); }
        }

        private int _problemsFixed;
        public int ProblemsFixed
        {
            get { return _problemsFixed; }
            set { _problemsFixed = value; RaisePropertyChanged("ProblemsFixed"); }
        }

        public AutoFixVm()
        {
            PatchResults = new ObservableCollection<PatchResultVM>();
            DetectProblemsProgress = new ProgressVM();
            SolveProblemsProgress = new ProgressVM();
        }

        internal void Reset()
        {
            DetectProblemsProgress.Reset();
            SolveProblemsProgress.Reset();
            ProblemsFound = 0;
            ProblemsFixed = 0;
            App.Current.Dispatcher.Invoke(() => PatchResults.Clear());
        }
    }

    public class PercentageConverter : IValueConverter
    {
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
