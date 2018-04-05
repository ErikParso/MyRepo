using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kros.TroubleShooterClient.Model;

namespace Kros.TroubleShooterClient.ViewModel
{
    /// <summary>
    /// form mode view model
    /// </summary>
    public class QuestionMode : ObservableObject
    {
        /// <summary>
        /// actual question text
        /// </summary>
        private string _actualQuestion = "";
        /// <summary>
        /// actualk question text
        /// </summary>
        public string ActualQuestion
        {
            get { return _actualQuestion; }
            set { _actualQuestion = value; RaisePropertyChanged("ActualQuestion"); }
        }

        /// <summary>
        /// Question link - you can navigate back to questions using this
        /// </summary>
        public ObservableCollection<Question> QuestionLink { get; set; }

        /// <summary>
        /// answers for current questions 
        /// </summary>
        public ObservableCollection<Answer> Answers { get; set; }

        /// <summary>
        /// init this view model
        /// </summary>
        /// <param name="rootQuestion"></param>
        public QuestionMode(Question rootQuestion)
        {
            QuestionLink = new ObservableCollection<Question>() { rootQuestion };         
            Answers = new ObservableCollection<Answer>();
            updateByCurentQuestion();
        }

        /// <summary>
        /// patches which solves current question problem 
        /// </summary>
        public IEnumerable<Patch> Patches { get { return ((StopQuestion)QuestionLink.Last()).Solutions; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="answer"></param>
        public void SelectAnswer(Answer answer)
        {
            Question subq = QuestionLink.Last().getQuestionByAnswer(answer.Id);        
            if (!QuestionLink.Contains(subq))
            {
                QuestionLink.Add(subq);
                updateByCurentQuestion();
            }
        }

        /// <summary>
        /// select question from question link
        /// </summary>
        /// <param name="question"></param>
        public void SelectBack(Question question)
        {
            while (QuestionLink.Last() != question)
                QuestionLink.Remove(QuestionLink.Last());
            updateByCurentQuestion();
        }

        /// <summary>
        /// updates model by current questions
        /// </summary>
        private void updateByCurentQuestion()
        {
            Answers.Clear();
            //last question in question link is actual
            Question rootQuestion = QuestionLink.Last();
            if (rootQuestion == null)
            {
                ActualQuestion = "K dispozícii nie sú žiadne otázky.";
                return;
            }
            ActualQuestion = rootQuestion.Text;
            //actualise answers by current question
            foreach (KeyValuePair<int, Model.Answer> answ in rootQuestion.PossibleAnswers)
                Answers.Add(new Answer()
                {
                    Id = answ.Key,
                    Text = answ.Value.MainText,
                    Detail = answ.Value.Detail,
                    //answer is available if has defined subquestion or stopQuestion
                    Available = rootQuestion.getQuestionByAnswer(answ.Key) != null
                });
        }

        /// <summary>
        /// resets model and navigates back to root question
        /// </summary>
        public void Reset()
        {
            Question root = QuestionLink.First();
            QuestionLink.Clear();
            QuestionLink.Add(root);
            updateByCurentQuestion();
        }

        /// <summary>
        /// the answer model
        /// </summary>
        public class Answer
        {
            /// <summary>
            /// answer identificator
            /// </summary>
            public int Id { get; set; }

            /// <summary>
            /// answer text
            /// </summary>
            public string Text { get; set; }

            /// <summary>
            /// Some answers can be morte complex
            /// </summary>
            public string Detail { get; set; }

            /// <summary>
            /// if answer is available
            /// </summary>
            public bool Available { get; set; }

            /// <summary>
            /// determine if answer is detailed
            /// </summary>
            public bool HasDetail {
                get { return !string.IsNullOrEmpty(Detail); }
            }
        }
    }
}
