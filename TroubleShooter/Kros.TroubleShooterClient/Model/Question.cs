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
        /// all answers.
        /// </summary>
        private Dictionary<int, Answer> _answers;

        /// <summary>
        /// the dictionary of possible answers.
        /// </summary>
        public Dictionary<int, Answer> PossibleAnswers =>
            _answers.Where((kp) => AnyPatchUnderAnswer(kp.Key))
            .ToDictionary(p => p.Key, p => p.Value);

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
        /// creates question
        /// </summary>
        public Question()
        {
            _answers = new Dictionary<int, Answer>();
            registerAnswers(_answers);
        }

        /// <summary>
        /// determines if there are usable patches somewhere under answer.
        /// </summary>
        /// <param name="answerId"></param>
        /// <returns></returns>
        private bool AnyPatchUnderAnswer(int answerId)
        {
            Question subquestion = getQuestionByAnswer(answerId);
            if (subquestion == null)
                return false;
            if (subquestion is StopQuestion)
                return true;
            foreach (int answer in subquestion.PossibleAnswers.Keys)
                if (subquestion.AnyPatchUnderAnswer(answer))
                    return true;
            return false;
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
        protected abstract void registerAnswers(Dictionary<int, Answer> possibleAnswers);
    }

    /// <summary>
    /// Decorate the root question with this attribute.
    /// </summary>
    public class RootQuestionAttribute : Attribute
    {

    }

    public class Answer
    {
        public string MainText { get; }
        public string Detail { get; }

        public Answer(string mainText, string detail = null)
        {
            MainText = mainText;
            Detail = detail;
        }
    }
}
