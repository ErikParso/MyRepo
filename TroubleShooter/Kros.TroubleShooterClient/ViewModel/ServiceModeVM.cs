using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// model for servis mode
    /// </summary>
    public class ServiceModeVM : ObservableObject
    {
        private bool _buttonEnabled;

        /// <summary>
        /// the list of servis parameters
        /// </summary>
        public List<OptionalServiceProp> Properties { get; private set; }

        /// <summary>
        /// Button send should be enablen only if all info is valid.
        /// </summary>
        public bool ButtonEnabled { get => _buttonEnabled; set { _buttonEnabled = value; RaisePropertyChanged("ButtonEnabled"); } }

        public void RefreshButton()
        {
            RaisePropertyChanged("ButtonEnabled");
        }

        /// <summary>
        /// init view model 
        /// </summary>
        public ServiceModeVM()
        {
            ButtonEnabled = true;
            Properties = new List<OptionalServiceProp>();
            foreach (ServisObject o in TroubleShooter.Current.RunData.ServisObjects.Values)
            {
                OptionalServiceProp prop = new OptionalServiceProp()
                {
                    Name = o.Title,
                    Editable = o.Editable,
                    IsPath = o.IsPath,
                    PossibleValues = o.PossibleValues?.ToList(),
                    Value = o.Value,
                    LargeText = o.LargeText,
                    Mandatory = o.Mandatory
                };
                Properties.Add(prop);
            }

            if (Properties.Count == 0)
            {
                //ak neboli properties nastavene aplikaciou nastav defaultne
                //zobrazia sa ked spustis apku standalone
                Properties.Add(new OptionalServiceProp()
                {
                    Name = "Zákaznícke číslo alebo kontakt",
                    Editable = true, Mandatory = true
                });
                Properties.Add(new OptionalServiceProp()
                {
                    Name = "Popis problému",
                    LargeText = true,
                    Mandatory = true
                });
            }
                
        }
    }
}
