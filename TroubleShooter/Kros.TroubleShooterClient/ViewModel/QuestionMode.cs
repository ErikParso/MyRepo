using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using Kros.TroubleShooterClient.Model;

namespace Kros.TroubleShooterClient.ViewModel
{
    public class QuestionMode : ObservableObject
    {
        private string _actualQuestion = "";
        public string ActualQuestion
        {
            get { return _actualQuestion; }
            set { _actualQuestion = value; RaisePropertyChanged("ActualQuestion"); }
        }

        public ObservableCollection<Question> QuestionLink { get; set; }

        public ObservableCollection<Answer> Answers { get; set; }

        public QuestionMode(Question rootQuestion)
        {
            QuestionLink = new ObservableCollection<Question>() { rootQuestion };
            Answers = new ObservableCollection<Answer>();
            updateByCurentQuestion();
        }

        public IEnumerable<Patch> Patches { get { return QuestionLink.Last().Solutions; } }

        public void SelectAnswer(Answer answer)
        {
            Question subq = QuestionLink.Last().getQuestionByAnswer(answer.Id);        
            if (!QuestionLink.Contains(subq))
            {
                QuestionLink.Add(subq);
                updateByCurentQuestion();
            }
        }

        public void SelectBack(Question question)
        {
            while (QuestionLink.Last() != question)
                QuestionLink.Remove(QuestionLink.Last());
            updateByCurentQuestion();
        }

        private void updateByCurentQuestion()
        {
            Question question = QuestionLink.Last();
            ActualQuestion = question.Text;
            Answers.Clear();
            foreach (KeyValuePair<int, string> answ in question.PossibleAnswers)
                Answers.Add(new Answer()
                {
                    Id = answ.Key,
                    Text = answ.Value,
                    Available = question.getQuestionByAnswer(answ.Key) != null
                });
        }

        public void Reset()
        {
            Question root = QuestionLink.First();
            QuestionLink.Clear();
            QuestionLink.Add(root);
            updateByCurentQuestion();
        }

        public class Answer
        {
            public int Id { get; set; }
            public string Text { get; set; }
            public bool Available { get; set; }
        }
    }
}
