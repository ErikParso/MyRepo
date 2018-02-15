using System;

namespace Kros.TroubleShooterClient.ViewModel
{
    public class OptionalServiceProp : ObservableObject
    {
        private string name;
        private string value;
        private bool editable = true;
        private bool isPath = false;

        public string Name { get => name; set { this.name = value; RaisePropertyChanged("Name"); } }
        public string Value { get => value; set { this.value = value; RaisePropertyChanged("Value"); } }
        public bool Editable { get => editable; set { editable = value; RaisePropertyChanged("Editable"); } }
        public bool IsPath { get => isPath; set { isPath = value; RaisePropertyChanged("IsPath"); } }
    }
}