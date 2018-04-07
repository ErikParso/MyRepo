using Kros.TroubleShooterClient.Model;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// Patch view model
    /// </summary>
    public class PatchResultVM : ObservableObject
    {
        private Patch _patch;
        private bool _canExecute;
        private ExecutionResult _executionResult;

        /// <summary>
        /// Execute checkbox value to bind.
        /// </summary>
        public bool CanExecute
        {
            get { return _canExecute; }
            set { _canExecute = value; RaisePropertyChanged("CanExecute"); }
        }

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
        public ExecutionResult ExecutionResult
        {
            get { return _executionResult; }
            set { _executionResult = value; RaisePropertyChanged("ExecutionResult"); }
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
            HelpHtml = patch.Instruction;
            ExecutionResult = ExecutionResult.NOT_EXECUTED;
            CanExecute = true;
        }

        /// <summary>
        /// executes patch
        /// </summary>
        public void ExecutePatch()
        {
            if (CanExecute)
            {
                if (_patch.SolveProblemSafe())
                    ExecutionResult = ExecutionResult.FIXED;
                else if (HelpHtml != null)
                    ExecutionResult = ExecutionResult.INSTRUCTOR;
                else
                    ExecutionResult = ExecutionResult.NOT_FIXED;
            }
        }

        public bool InstructionsResult()
        {
            return _patch.ControlProblemSafe();
        }
    }

    public enum ExecutionResult
    {
        NOT_EXECUTED, FIXED, NOT_FIXED, INSTRUCTOR
    }
}
