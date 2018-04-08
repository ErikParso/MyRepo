//version(2018022620)

using System;
using System.Threading;
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class TestPatch1 : Patch
    {
        public override string Description => "Problém sa podarí identifikovať aj odstrániť.";
        public override string PatchName => "Problém variant 1";
        public override string Instruction => null;
        protected override bool ComplexIdentify() => true;
        protected override bool FastIdentify(RunData runData) => true;
        protected override bool SolveProblem(RunData data) => true;
        protected override bool ControlProblem(RunData runData) => false;
    }

    public class TestPatch2 : Patch
    {
        public override string Description => "Problém sa podarí identifikovať ale nepodarí sa ho odstrániť. K dispozícii je funkčný návod na jeho odstránenie.";
        public override string PatchName => "Problém variant 2";
        public override string Instruction => "Funkčný návod na odstránenie problému";
        protected override bool ComplexIdentify() => true;
        protected override bool FastIdentify(RunData runData) => false;
        protected override bool SolveProblem(RunData data) => false;
        protected override bool ControlProblem(RunData runData) => true;
    }

    public class TestPatch3 : Patch
    {
        public override string Description => "Problém sa podarí identifikovať ale nie odstrániť. Návod nie je k dispozícii.";
        public override string PatchName => "Problém variant 3";
        public override string Instruction => null;
        protected override bool ComplexIdentify() => true;
        protected override bool FastIdentify(RunData runData) => false;
        protected override bool SolveProblem(RunData data) => false;
        protected override bool ControlProblem(RunData runData) => false;
    }

    public class TestPatch4 : Patch
    {
        public override string Description => "Problém sa podarí identifikovať ale nepodarí sa ho odstrániť. K dispozícii je návod na jeho odstránenie ale užívateľ s ním má problém.";
        public override string PatchName => "Problém variant 4";
        public override string Instruction => "Funkčný návod na odstránenie problému";
        protected override bool ComplexIdentify() => true;
        protected override bool FastIdentify(RunData runData) => false;
        protected override bool SolveProblem(RunData data) => false;
        protected override bool ControlProblem(RunData runData) => false;
    }
}
