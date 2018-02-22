using System.Collections.Generic;

namespace Kros.TroubleShooterClient.Model
{
    public class StopQuestion : Question
    {
        public override string Text => null;

        public override string Category => null;

        public List<Patch> Solutions { get; private set; }

        public StopQuestion(params Patch[] solutions)
        {
            Solutions = new List<Patch>(solutions);
        }

        public override Question getQuestionByAnswer(int answer)
        {
            return null;
        }

        protected override void registerAnswers(Dictionary<int, string> possibleAnswers)
        {

        }
    }
}
