//version(2018022721)

using System.IO;
using System.Threading;
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class AdobeSecurityPatch : Patch
    {
        public override string Description => "V Olympe sú originálne tlačivá vo formáte PDF a zobrazujú sa pomocou Adobe Reader, ktorý je nainštalovaný v počítači. Po aktualizácií Adobe Reader sa stáva, že sa automaticky zmenia nastavenia zabezpečenia. Pri tlači originálnych tlačív sa potom môže zobraziť hláška \"Užívateľ operáciu zrušil\".";

        public override string PatchName => "Adobe Reader a originálne tlačivá";

        public override string Instruction
        {
            get
            {
                InstructionBuilder b = new InstructionBuilder(Description + "<br>" + "Pre korektné zobrazovanie PDF dokumentov odporúčame upraviť nastavenia nasledovne:");
                b.Points.Add("Cez tlačidlo ŠTART si zobrazíme všetky programy a spustíme Adobe Reader.");
                b.Points.Add("Cez menu Úpravy zvolíme Predvoľby.");
                b.Points.Add("Klikneme na záložku zabezpečenie v ľavom paneli.");
                b.Points.Add("Následne odznačíme Povoliť chránený režim pri spustení a Povoliť zvýšené zabezpečenie.");
                b.Points.Add("V sekcii privilégované umiestnenia pridáme cestu k priečinku, kde máme nainštalovaný program Olymp (zvyčajne C:\\Olymp).");
                b.Points.Add("Úprava uvedených nastavení si nevyžaduje reštart počítača. Detailnejší popis riešenia problému nájdete v linkoch.");
                b.NumberedPoints = true;
                b.AdFaqLik("detailný postup s obrázkami", 1258);
                return b.ToString();
            }
        }


        protected override bool ComplexIdentify()
        {
            Thread.Sleep(1000);
            return true;
        }

        protected override bool SolveProblem(RunData data)
        {
            Thread.Sleep(1000);
            return true;
        }

        protected override bool FastIdentify(RunData runData)
        {
            return false;
        }

        protected override bool ControlProblem(RunData runData)
        {
            return true;
        }
    }
}
