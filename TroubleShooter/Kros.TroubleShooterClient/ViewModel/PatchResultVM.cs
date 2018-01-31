using Kros.TroubleShooterClient.Model;

namespace Kros.TroubleShooterClient.ViewModel
{
    public class PatchResultVM : ObservableObject
    {
        private Patch _patch;

        public string Name { get; set; }

        public string Description { get; set; }

        private bool _selected;
        public bool Selected { get { return _selected; } set { _selected = value; RaisePropertyChanged("Selected"); } }

        private bool? _problemFixed;
        public bool? ProblemFixed {
            get { return _problemFixed; }
            set { _problemFixed = value; RaisePropertyChanged("ProblemFixed"); }
        }

        private string _executionResult;
        public string ExecutionResult
        {
            get { return _executionResult; }
            set { _executionResult = value;  RaisePropertyChanged("ExecutionResult"); }
        }

        public PatchResultVM(Patch patch, bool makeSelected)
        {
            Selected = makeSelected;
            _patch = patch;
            Name = patch.PatchName;
            Description = patch.Description;
            ExecutionResult = "";
            ProblemFixed = null;
        }

        public void ExecutePatch()
        {
            ProblemFixed =  _patch.SolveProblemSafe();
            if (ProblemFixed == true)
                ExecutionResult += "Zdá sa, že oprava chyby prebehla úspešne.";
            else
                ExecutionResult += "Problém sa nepodarilo opraviť.";
        }
    }
}
