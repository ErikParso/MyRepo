using System;
using System.Collections.Generic;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// Describes servis propery 
    /// </summary>
    public class OptionalServiceProp : ObservableObject
    {
        private string name;
        private string value;
        private bool editable = true;
        private bool isPath = false;
        private List<string> possibleValues;

        /// <summary>
        /// property name will be displayed as a label on form
        /// </summary>
        public string Name { get => name; set { this.name = value; RaisePropertyChanged("Name"); } }
        /// <summary>
        /// property value - displayed in a textbox
        /// can be set default
        /// </summary>
        public string Value { get => value; set { this.value = value; RaisePropertyChanged("Value"); } }
        /// <summary>
        /// if property is editable - for programaticaly sat values
        /// </summary>
        public bool Editable { get => editable; set { editable = value; RaisePropertyChanged("Editable"); } }
        /// <summary>
        /// if true the file explorer button is enabled
        /// </summary>
        public bool IsPath { get => isPath; set { isPath = value; RaisePropertyChanged("IsPath"); } }
        /// <summary>
        /// if set - values are displayed in combobox
        /// </summary>
        public List<string> PossibleValues { get => possibleValues; set { possibleValues = value; RaisePropertyChanged("PossibleValues"); } }
        /// <summary>
        /// true - combobox with possibleValues
        /// false - textbox - free text
        /// </summary>
        public bool SelectText { get { return possibleValues != null; } }
    }
}