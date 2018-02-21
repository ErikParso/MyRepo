using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// view models are implementing this class
    /// </summary>
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(params string[] propertyName)
        {
            if (PropertyChanged != null)
                foreach(string prop in propertyName)
                    PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
