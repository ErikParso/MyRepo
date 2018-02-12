using System;

namespace Kros.TroubleShooterClient.Model
{
    /// <summary>
    /// Patch should resolve some kind of problem - <see cref="RegisterToProblem(Problem)"/> 
    /// Patch chcechs if it can solve the problem - <see cref="IdentifyProblem()"/>
    /// Patch tries to solve the problem - <see cref="SolveProblem()"/> 
    public abstract class Patch
    {
        /// <summary>
        /// The patch Description.
        /// </summary>
        public abstract string Description { get; }

        /// <summary>
        /// The patch Name.
        /// </summary>
        public abstract string PatchName { get; }

        /// <summary>
        /// The patch Name.
        /// </summary>
        public abstract string HtmlInfo { get; }

        /// <summary>
        /// The id of problem which patch solves
        /// </summary>
        public abstract int SolvesProblem { get; }

        /// <summary>
        /// Identifies, if the patch can solve the problem.
        /// </summary>
        protected abstract bool IdentifyProblem();

        /// <summary>
        /// Runs problem identification with exception handling.
        /// </summary>
        /// <returns></returns>
        public bool IdentifyProblemSafe()
        {
            try
            {
                return IdentifyProblem();
            }
            catch (Exception e)
            {
                Logger.LogIdentifyException(e, this, Logger.Mode.IDENTIFICATION);
                return false;
            }
        }

        /// <summary>
        /// Patch tries to solve the problem.
        /// </summary>
        /// <returns>result</returns>
        protected abstract bool SolveProblem();

        /// <summary>
        /// Runs problem solving method with exception handling
        /// </summary>
        /// <returns></returns>
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
