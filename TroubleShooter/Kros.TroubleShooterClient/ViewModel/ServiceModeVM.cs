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
        /// <summary>
        /// the list of servis parameters
        /// </summary>
        public List<OptionalServiceProp> Properties { get; private set; }

        /// <summary>
        /// init view model 
        /// </summary>
        public ServiceModeVM()
        {
            Properties = new List<OptionalServiceProp>();
            foreach (ServisObject o in TroubleShooter.Current.RunData.ServisObjects.Values)
            {
                Properties.Add(new OptionalServiceProp
                {
                    Name = o.Title,
                    Editable = o.Editable,
                    IsPath = o.IsPath,
                    PossibleValues = o.PossibleValues?.ToList(),
                    Value = o.Value
                });
            }

            if (Properties.Count == 0)
            {
                //ak neboli properties nastavene aplikaciou nastav defaultne
                //zobrazia sa ked spustis apku standalone
                Properties.Add(new OptionalServiceProp()
                {
                    Name = "Kontakt",
                });
                Properties.Add(new OptionalServiceProp()
                {
                    Name = "Popis problému",
                });
            }
        }        
    }
}
