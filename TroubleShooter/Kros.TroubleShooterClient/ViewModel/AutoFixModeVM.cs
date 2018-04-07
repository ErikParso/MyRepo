using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// model for auto fix control
    /// </summary>
    public class AutoFixVm : ObservableObject
    {
        private bool _canRunForm;
        private bool _canExecute;
        private bool _buttonsEnabled;
        private bool _hasProblems;

        public ObservableCollection<PatchResultVM> PatchResults { get; set; }

        public ProgressVM Progress { get; private set; }

        public bool CanExecute
        {
            get { return _canExecute && CheckedAll != false; }
            set { _canExecute = value; RaisePropertyChanged("CanExecute"); }
        }

        public bool CanRunForm
        {
            get { return _canRunForm; }
            set { _canRunForm = value; RaisePropertyChanged("CanRunForm"); }
        }

        public bool? CheckedAll
        {
            get
            {
                if (PatchResults.Where(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED).All(p => p.CanExecute))
                    return true;
                else if (PatchResults.Where(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED).All(p => !p.CanExecute))
                    return false;
                else
                    return null;
            }
            set
            {
                foreach (PatchResultVM p in PatchResults)
                    p.CanExecute = (bool)value;
                RaisePropertyChanged("CheckedAll");
            }
        }

        public bool CheckEnabled
        {
            get { return PatchResults.Any(p => p.ExecutionResult == ExecutionResult.NOT_EXECUTED); }
        }

        public bool HasProblems
        {
            get { return _hasProblems; }
            set { _hasProblems = value; RaisePropertyChanged("HasProblems"); }
        }

        public bool ButtonsEnabled
        {
            get { return _buttonsEnabled; }
            set { _buttonsEnabled = value; RaisePropertyChanged("ButtonsEnabled"); }
        }

        public AutoFixVm()
        {
            PatchResults = new ObservableCollection<PatchResultVM>();
            Progress = new ProgressVM();
        }

        internal void Reset()
        {
            Progress.Reset();
            App.Current.Dispatcher.Invoke(() => PatchResults.Clear());
        }

        public void RefreshCheck()
        {
            RaisePropertyChanged("CheckedAll");
            RaisePropertyChanged("CheckEnabled");
        }
    }
}
