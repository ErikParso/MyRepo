//version(2018022305)
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;
using System.IO;

namespace Kros.TroubleShooterClient.Patches
{
    public class ChybaZdvihuAccessPatch : Patch
    {
        public override string Description => "Počas poslednej úpravy verzie databázy nastala chyba. Databáza je poškodená a troubleshooter sa pokúsi obnoviť ju zo zálohy.";

        public override string PatchName => "Poškodená databáza";

        public override string Instruction => null;


        protected override bool FastIdentify(RunData data)
        {
            return data.HasFlag("zdvih_nedobehol_access");
        }

        protected override bool ComplexIdentify()
        {
            //problem can be identified only from run data provided by olymp
            return false;
        }

        protected override bool SolveProblem(RunData data)
        {
            string zal = Path.ChangeExtension(data.Get("broken_db"),".zal");
            if (!File.Exists(zal))
            {
                ExecutionResult = "Troubleshooter nenašiel zálohu";
                return false;
            }
            ExecutionResult = "Databázu sa nepodarilo obnoviť zo zálohy";
            File.Delete(data.Get("broken_db"));
            File.Copy(zal, Path.ChangeExtension(zal, ".mdb"));
            ExecutionResult = "Databáza bola úspešne obnovená zo zálohy.";
            return true;
        }
    }
}
