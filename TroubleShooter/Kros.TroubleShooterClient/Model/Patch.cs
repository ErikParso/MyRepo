using Kros.TroubleShooterInput;
using System;

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
        /// The execution result
        /// </summary>
        public string ExecutionResult { get; set; }

        /// <summary>
        /// Identifies, the prolem on computer
        /// Somes problems identifications lasts longer and neednt data from hosting app.
        /// If uou want identify problem using input data, use <see cref="FastIdentify(RunData)"/> method.
        /// Identifications will be used only in complex mode troubleshooter. 
        /// </summary>
        protected abstract bool ComplexIdentify();

        /// <summary>
        /// runs <see cref="ComplexIdentify(RunData)"/> but also handles exceptions
        /// </summary>
        /// <returns>
        /// true - problem identified and this patch should be used to solve it
        /// false - problem unidentified or identification failed
        /// </returns>
        public bool ComplexIdentifySafe()
        {
            try
            {
                return ComplexIdentify();
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
        protected abstract bool SolveProblem(RunData runData);

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
                return SolveProblem(TroubleShooter.Current.RunData);
            }
            catch (Exception e)
            {
                Logger.LogIdentifyException(e, this, Logger.Mode.SOLVING);
                return false;
            }
        }

        /// <summary>
        /// Identifies problem realy fast from input data defined by hosting app.
        /// Will be used in both modes complex and minimalistic. 
        /// Its Prefered type of problem identification.
        /// </summary>
        protected abstract bool FastIdentify(RunData runData);

        /// <summary>
        /// runs <see cref="FastIdentify(RunData)"/> but also handles exceptions
        /// </summary>
        /// <returns>
        /// true - problem identified and this patch should be used to solve it
        /// false - problem unidentified or identification failed
        /// </returns>
        public bool FastIdentifySafe()
        {
            try
            {
                return FastIdentify(TroubleShooter.Current.RunData);
            }
            catch (Exception e)
            {
                Logger.LogIdentifyException(e, this, Logger.Mode.FAST_IDENTIFICATION);
                return false;
            }
        }
    }
}
