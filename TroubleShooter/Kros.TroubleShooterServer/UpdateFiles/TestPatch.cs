//version(2018022620)

using System;
using System.Threading;
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class TestPatch : Patch
    {
        public override string Description => "test patch";

        public override string PatchName => "test patch";

        public override string Instruction => "navod";


        protected override bool ComplexIdentify()
        {
            return true;
        }

        protected override bool FastIdentify(RunData runData)
        {
            return false;
        }

        protected override bool SolveProblem(RunData data)
        {
            return false;
        }

        protected override bool ControlProblem(RunData runData)
        {
            return true;
        }
    }

    public class TestPatch2 : Patch
    {
        public override string Description => "test patch";

        public override string PatchName => "test patch";

        public override string Instruction => "navod";


        protected override bool ComplexIdentify()
        {
            return true;
        }

        protected override bool FastIdentify(RunData runData)
        {
            return false;
        }

        protected override bool SolveProblem(RunData data)
        {
            return true;
        }

        protected override bool ControlProblem(RunData runData)
        {
            return false;
        }
    }
}
