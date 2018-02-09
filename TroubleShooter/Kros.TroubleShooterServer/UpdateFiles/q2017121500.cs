//version(2018020802)

using Kros.TroubleShooterClient.Model;
using System.Collections.Generic;

namespace Kros.TroubleShooterClient.Patches
{
    [RootQuestion]
    public class RootQuestion : Question
    {
        public override string Category { get { return "Olymp"; } }

        public override int Id { get { return 2017121500; } }

        public override string Text { get { return "V akej časti programu sa vyskytuje problém ? v2"; } }

        protected override void registerAnswers(Dictionary<int, string> possibleAnswers)
        {
            possibleAnswers.Add(0, "Personalistika");
            possibleAnswers.Add(1, "Mzdy");
            possibleAnswers.Add(2, "Tlač");
            possibleAnswers.Add(3, "Spustenie Olympu");
        }

        public override Question getQuestionByAnswer(int answerIndex)
        {
            switch (answerIndex)
            {
                case 0: return new Q2017121501();
                case 2: return new Q2017121502();
                default: return null;
            }
        }
    }

    public class Q2017121501 : Question
    {
        public override string Category { get { return "Personalistika"; } }

        public override int Id { get { return 2017121501; } }

        public override string Text { get { return "V akej časti personalistiky máte problém ?"; } }

        protected override void registerAnswers(Dictionary<int, string> possibleAnswers)
        {
            possibleAnswers.Add(0, "Grid");
            possibleAnswers.Add(1, "Bočná karta");
            possibleAnswers.Add(2, "Pridávanie zamestnanca");
        }

        public override Question getQuestionByAnswer(int answerIndex)
        {
            return null;
        }
    }

    public class Q2017121502 : Question
    {
        public override string Category { get { return "Tlač"; } }

        public override int Id { get { return 2017121502; } }

        public override string Text { get { return "Inštalovali ste v poslednej dobe Alfu ?"; } }

        protected override void registerAnswers(Dictionary<int, string> possibleAnswers)
        {
            possibleAnswers.Add(0, "Áno");
            possibleAnswers.Add(1, "Nie");
        }

        public override Question getQuestionByAnswer(int answerIndex)
        {
            switch (answerIndex)
            {
                case 0: return new Q2017121503();
                default: return null;
            }
        }
    }

    public class Q2017121503 : Question
    {
        public override string Category { get { return "Fonty"; } }

        public override int Id { get { return 2017121503; } }

        public override string Text { get { return ""; } }

        protected override void registerAnswers(Dictionary<int, string> possibleAnswers)
        {

        }

        public override Question getQuestionByAnswer(int answerIndex)
        {
            return null;
        }
    }
}
