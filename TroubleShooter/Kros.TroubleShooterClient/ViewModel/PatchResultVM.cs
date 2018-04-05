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
        private ExecutionResult _executionResult;
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
        }

        /// <summary>
        /// executes patch
        /// </summary>
        public void ExecutePatch()
        {
            if (_patch.SolveProblemSafe())
                ExecutionResult = ExecutionResult.FIXED;
            else if (HelpHtml != null)
                ExecutionResult = ExecutionResult.INSTRUCTOR;
            else
                ExecutionResult = ExecutionResult.NOT_FIXED;
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
