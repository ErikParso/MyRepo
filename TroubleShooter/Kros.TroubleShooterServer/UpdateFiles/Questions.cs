//version(2018022217)
using Kros.TroubleShooterClient.Model;
using System.Collections.Generic;

namespace Kros.TroubleShooterClient.Patches
{
    [RootQuestion]
    public class RootQuestion : Question
    {
        public override string Category => "Olymp";
        public override string Text => "V akej časti programu sa vyskytuje problém ?";
        protected override void registerAnswers(Dictionary<int, Answer> possibleAnswers)
        {
            possibleAnswers.Add(0, new Answer("Personalistika"));
            possibleAnswers.Add(1, new Answer("Mzdy"));
            possibleAnswers.Add(2, new Answer("Tlač"));
            possibleAnswers.Add(3, new Answer("Spustenie Olympu"));
        }
        public override Question getQuestionByAnswer(int answerIndex)
        {
            switch (answerIndex)
            {
                case 0: return new Personalistika();
                case 2: return new TlacChybyPopis();
                default: return null;                 
            }
        }
    }

    public class Personalistika : Question
    {
        public override string Category => "Personalistika";
        public override string Text => "V akej časti personalistiky máte problém ?";
        protected override void registerAnswers(Dictionary<int, Answer> possibleAnswers)
        {
            possibleAnswers.Add(0, new Answer("Grid"));
            possibleAnswers.Add(1, new Answer("Bočná karta"));
            possibleAnswers.Add(2, new Answer("Pridávanie zamestnanca"));
        }
        public override Question getQuestionByAnswer(int answerIndex)
        {
            return null;
        }
    }

    public class TlacChybyPopis : Question
    {
        public override string Text => "V ktorej časti tlače sa prejavuje problém ?";
        public override string Category => "Tlač";
        public override Question getQuestionByAnswer(int answer)
        {
            switch (answer)
            {
                case 1: return new StopQuestion(new AdobeSecurityPatch());
                case 2: return new StopQuestion(new PravaManZostPatch());
                default: return null;
            }
        }
        protected override void registerAnswers(Dictionary<int, Answer> possibleAnswers)
        {
            possibleAnswers.Add(1, new Answer("Chybová hláška programu Adobe Reader.", "V prípade originálnych pdf tlačív sa mi zobrazuje hláška \"Užívateľ operáciu zrušil\"."));
            possibleAnswers.Add(2, new Answer("Tlač manažérskych zostáv.", "Mám chybové hlásenie 710-3554-x1006-1004 Office has detected a problem with this file."));
        }
    }
}
