//version(2018022204)
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class TestPatch : Patch
    {
        public override string Description { get { return "Testovaci patch"; } }

        public override string PatchName { get { return "Test"; } }

        public override string HtmlInfo { get { return null; } }

        protected override bool ComplexIdentify()
        {
            return true;
        }

        protected override bool SolveProblem(RunData data)
        {
            return true;
        }

        protected override bool FastIdentify(RunData runData)
        {
            return true;
        }
    }
}
