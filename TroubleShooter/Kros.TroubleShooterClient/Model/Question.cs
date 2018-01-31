using System;
using System.Collections.Generic;
using System.Linq;

namespace Kros.TroubleShooterClient.Model
{
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
        /// The problem category
        /// </summary>
        public abstract string Category { get; }
        /// <summary>
        /// problem identifier, use format YYYYMMDDxx
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
        /// </summary>
        /// <param name="answer">users answer to question</param>
        /// <returns>null or subproblem</returns>
        public abstract Question getQuestionByAnswer(int answer);

        /// <summary>
        /// register
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

    public class RootQuestionAttribute : Attribute
    {

    }
}
