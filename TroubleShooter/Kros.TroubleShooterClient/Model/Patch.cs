using Kros.TroubleShooterInput;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Windows;

namespace Kros.TroubleShooterClient.Model
{
    /// <summary>
    /// Patch should resolve some kind of problem - <see cref="SolvesProblem"/> 
    /// Patch chcecks if it can solve the problem - <see cref="IdentifyProblem(RunData)"/>
    /// Patch tries to solve the problem - <see cref="SolvesProblem(RunData)"/> 
    public abstract class Patch
    {
        /// <summary>
        /// The patch Description. The brief explanation of this patch.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The patch Name. Patch Title.
        /// </summary>
        public abstract string PatchName { get; }

        /// <summary>
        /// Detail information to this patch. Supports html tags.
        /// Detail is not provided if is HtmlInfo null.
        /// </summary>
        public abstract string HtmlInfo { get; }

        /// <summary>
        /// The id of problem which this patch solves.
        /// Should correspond with <see cref="Question.Id"/>.
        /// </summary>
        public abstract int SolvesProblem { get; }

        /// <summary>
        /// The execution result
        /// </summary>
        public string ExecutionResult { get; set; }

        /// <summary>
        /// Identifies, if the patch can solve the problem.
        /// </summary>
        protected abstract bool IdentifyProblem(RunData runData);

        /// <summary>
        /// Runs problem identification with exception handling.
        /// </summary>
        /// <returns>
        /// true - problem identified and this patch can solve it
        /// false - problem unidetified or identification failed
        /// </returns>
        public bool IdentifyProblemSafe()
        {
            try
            {
                return IdentifyProblem(TroubleShooter.Current.RunData);
            }
            catch (Exception e)
            {
                Logger.LogIdentifyException(e, this, Logger.Mode.IDENTIFICATION);
                return false;
            }
        }

        /// <summary>
        /// Tries to solve the problem.
        /// </summary>
        protected abstract bool SolveProblem();

        /// <summary>
        /// Runs problem solving with exception handling.
        /// </summary>
        /// <returns>
        /// true - problem was sucessfully fixed
        /// false - problem cant be fixed or repair process failed.
        /// </returns>
        public bool SolveProblemSafe()
        {
            try
            {
                return SolveProblem();
            }
            catch (Exception e)
            {
                Logger.LogIdentifyException(e, this, Logger.Mode.SOLVING);
                return false;
            }
        }
    }
}
