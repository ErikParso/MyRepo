using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kros.TroubleShooterClient.ViewModel
{
    public class ProgressVM : ObservableObject
    {
        private string _actualWork = "";
        public string ActualWork
        {
            get { return _actualWork; }
            set { _actualWork = value; RaisePropertyChanged("ActualWork"); }
        }

        private int count = 0;
        public int Count { set { count = value; RaisePropertyChanged("Progress", "ProgressPercentage"); } }

        private int processed = 0;

        public string ProgressPercentage { get { return count == 0 ? "" : (processed * 100 / count) + " %"; } set { } }

        public double Progress { get { return count == 0 ? 0 : (double)processed / count; } set { } }

        public void Reset()
        {
            ActualWork = "";
            Count = 0;
            processed = 0;
        }

        public void Add(int ticks = 1)
        {
            processed += ticks;
            RaisePropertyChanged("Progress", "ProgressPercentage");
        }

        public void Set100()
        {
            count = 1;
            processed = 1;
            RaisePropertyChanged("Progress", "ProgressPercentage");
        }
    }
}
