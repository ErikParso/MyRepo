using Kros.TroubleShooterClient.Model;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// Patch view model
    /// </summary>
    public class PatchResultVM : ObservableObject
    {
        /// <summary>
        /// model
        /// </summary>
        private Patch _patch;

        /// <summary>
        /// Patch name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Patch description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// patch detail
        /// </summary>
        public string HelpHtml { get; set; }

        /// <summary>
        /// null - non executed
        /// true - patch fixed a problem
        /// false - patch didnt fix a problem
        /// </summary>
        private bool? _problemFixed;
        public bool? ProblemFixed {
            get { return _problemFixed; }
            set { _problemFixed = value; RaisePropertyChanged("ProblemFixed"); }
        }

        /// <summary>
        /// initialiases a new model
        /// </summary>
        /// <param name="patch">patch</param>
        /// <param name="makeSelected"></param>
        public PatchResultVM(Patch patch)
        {
            _patch = patch;
            Name = patch.PatchName;
            Description = patch.Description;
            HelpHtml = patch.HtmlInfo ;
            ProblemFixed = null;
        }

        /// <summary>
        /// executes patch
        /// </summary>
        public void ExecutePatch()
        {
            ProblemFixed = _patch.SolveProblemSafe();
        }
    }
}
