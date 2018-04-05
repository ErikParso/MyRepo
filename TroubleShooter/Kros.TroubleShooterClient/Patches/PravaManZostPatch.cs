//version(2018022701)
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class PravaManZostPatch : Patch
    {
        public override string Description => "Pri zobrazovaní manažérskych zostáv (tie sú zobrazované v exceli) vyhodí chybové hlásenie:  710-3554-x1006-1004 Office has detected a problem with this file. To help protect your computer this file cannot be opened (Office detekoval problém s týmto súborom. Kvôli ochrane vášho počítača súbor nemôže byť otvorený).";

        public override string PatchName => "Manažérske zostavy";

        public override string Instruction
        {
            get
            {
                InstructionBuilder b = new InstructionBuilder(Description + "<br>" + "Vtedy je potrebné v exceli pridať umiestnenie našich excelovských zostáv medzi dôveryhodné, aby ich excel načítal.");
                b.Points.Add("v zobrazenom excelovskom súbore cez menu Súbor – Možnosti a zobrazenom formulári kliknúť na Centrum dôveryhodnosti a klik na Nastavenia centra dôveryhodnosti.");
                b.Points.Add("Naľavo klik na Dôveryhodné umiestnenie a tu Pridať nové umiestnenie ");
                b.Points.Add(@"v zobrazenom okne s názvom Dôveryhodné umiestnenie balíka Microsoft Office zadať miesto, kde sú excelovské zostavy Olympu umiestnené – štandardne C:\Olymp\Reporty\Excel");
                b.NumberedPoints = true;
                b.AdFaqLik("detailný postup s obrázkami", 1150);
                return b.ToString();
            }
        }

        //problem sa neda identifikovat, iba na zaklade otazok a vyriesit si ho vie len uzivatel
        protected override bool ComplexIdentify() => false;
        protected override bool SolveProblem(RunData data) => false;
        protected override bool FastIdentify(RunData runData) => false;
    }
}
