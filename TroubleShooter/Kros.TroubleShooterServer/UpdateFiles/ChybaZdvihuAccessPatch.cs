//version(2018022209)
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;
using System.IO;

namespace Kros.TroubleShooterClient.Patches
{
    public class ChybaZdvihuAccessPatch : Patch
    {
        public override string Description { get { return "Nastala chyba pri zdvihu databázy."; } }

        public override string PatchName { get { return "Poškodená databáza"; } }

        public override string HtmlInfo { get { return null; } }


        protected override bool FastIdentify(RunData data)
        {
            //return data.ErrNumber > 1700 && data.ErrNumber < 1780;
            return true;
        }

        protected override bool ComplexIdentify()
        {
            //problem can be identified only from run data 
            return false;
        }

        protected override bool SolveProblem(RunData data)
        {
            return false;
        }
    }
}
