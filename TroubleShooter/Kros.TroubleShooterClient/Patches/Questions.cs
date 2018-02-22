//version(2018022207)
using Kros.TroubleShooterClient.Model;
using System.Collections.Generic;

namespace Kros.TroubleShooterClient.Patches
{
    [RootQuestion]
    public class RootQuestion : Question
    {
        public override string Category { get { return "Olymp"; } }

        public override string Text { get { return "V akej časti programu sa vyskytuje problém ?"; } }

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
                case 0: return new Personalistika();
                case 2: return new Tlac();
                case 3: return new StopQuestion(new ChybaZdvihuAccessPatch());
                // answers 1 3 unselectable.. not implemented
                default: return null;                 
            }
        }
    }

    public class Personalistika : Question
    {
        public override string Category { get { return "Personalistika"; } }

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

    public class Tlac : Question
    {
        public override string Category { get { return "Tlač"; } }

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
                case 0: return new StopQuestion(new PoskodeneFontyPatch());
                // answer 1 unselectable.. not implemented
                default: return null;
            }
        }
    }
}
