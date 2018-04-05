using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// model to progressbars
    /// </summary>
    public class ProgressVM : ObservableObject
    {
        /// <summary>
        /// actual work text
        /// </summary>
        private string _actualWork = "";
        /// <summary>
        /// actual work text
        /// </summary>
        public string ActualWork
        {
            get { return _actualWork; }
            set { _actualWork = value; RaisePropertyChanged("ActualWork"); }
        }

        /// <summary>
        /// max value
        /// </summary>
        private int count = 0;
        public int Count
        {
            get => count;
            set { count = value; RaisePropertyChanged("Progress", "ProgressPercentage"); }
        }

        /// <summary>
        /// actual value
        /// </summary>
        private int processed = 0;

        /// <summary>
        /// percentage text
        /// </summary>
        public string ProgressPercentage { get { return count == 0 ? "" : (processed * 100 / count) + " %"; } set { } }

        /// <summary>
        /// decimal progress
        /// </summary>
        public double Progress { get { return count == 0 ? 0 : (double)processed / count; } set { } }

        /// <summary>
        /// resets model
        /// </summary>
        public void Reset()
        {
            ActualWork = "";
            Count = 0;
            processed = 0;
        }

        /// <summary>
        /// add ticks to progress
        /// </summary>
        /// <param name="ticks"></param>
        public void Add(int ticks = 1)
        {
            processed += ticks;
            RaisePropertyChanged("Progress", "ProgressPercentage");
        }

        /// <summary>
        /// sets 100 % in progress 
        /// </summary>
        public void Set100()
        {
            count = 1;
            processed = 1;
            RaisePropertyChanged("Progress", "ProgressPercentage");
        }
    }
}
