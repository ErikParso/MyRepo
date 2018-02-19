using Kros.TroubleShooterClient.Model;

namespace Kros.TroubleShooterClient.ViewModel
{
    public class PatchResultVM : ObservableObject
    {
        private Patch _patch;

        public string Name { get; set; }

        public string Description { get; set; }

        public string HelpHtml { get; set; }

        private bool _selected;
        public bool Selected { get { return _selected; } set { _selected = value; RaisePropertyChanged("Selected"); } }

        private string executionResult;

        private bool? _problemFixed;
        public bool? ProblemFixed {
            get { return _problemFixed; }
            set { _problemFixed = value; RaisePropertyChanged("ProblemFixed"); }
        }

        public PatchResultVM(Patch patch, bool makeSelected)
        {
            Selected = makeSelected;
            _patch = patch;
            Name = patch.PatchName;
            Description = patch.Description;
            HelpHtml = patch.HtmlInfo ;
            ProblemFixed = null;
        }

        public void ExecutePatch()
        {
            ProblemFixed =  _patch.SolveProblemSafe();
        }
    }
}
