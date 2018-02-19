using System;
using System.Collections.Generic;
using System.Linq;

namespace Kros.TroubleShooterClient.Model
{
    /// <summary>
    /// The question tree structure which helps identify problem.
    /// </summary>
    public abstract class Question
    {
        /// <summary>
        /// the dictionary of possible answers
        /// </summary>
        public Dictionary<int,string> PossibleAnswers;
        
        /// <summary>
        /// Question text.
        /// </summary>
        public abstract string Text { get; }

        /// <summary>
        /// The problem category. Will be shown in problem path Question1 >> Question2 >> Question3
        /// so user can navigate back to specific question.
        /// </summary>
        public abstract string Category { get; }

        /// <summary>
        /// problem identifier. Should correspond with <see cref="Patch.SolvesProblem"/>
        /// </summary>
        public abstract int Id { get; }

        /// <summary>
        /// creates question
        /// </summary>
        public Question()
        {
            PossibleAnswers = new Dictionary<int, string>();
            registerAnswers(PossibleAnswers);
        }

        /// <summary>
        /// gets subquestion by answer. Return null if there is no subproblems.
        /// Answers are numbered in <see cref="registerAnswers(Dictionary{int, string})"/>
        /// </summary>
        /// <param name="answer">users answer to a question</param>
        /// <returns>null or subproblem</returns>
        public abstract Question getQuestionByAnswer(int answer);

        /// <summary>
        /// register possible answers.
        /// If no answers are registered its final question and corespondich patches will be executed.
        /// </summary>
        /// <param name="possibleAnswers"></param>
        protected abstract void registerAnswers(Dictionary<int,string> possibleAnswers);

        /// <summary>
        /// All patches registered to this problem
        /// </summary>
        public IEnumerable<Patch> Solutions
        {
            get
            {
                return TroubleShooter.Current.Patches.Where(p => p.SolvesProblem == Id);
            }
        }
    }

    /// <summary>
    /// Decorate the root question with this attribute.
    /// </summary>
    public class RootQuestionAttribute : Attribute
    {

    }
}
