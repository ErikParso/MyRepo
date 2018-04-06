//version(2018022620)
using Kros.TroubleShooterClient.Model;
using Microsoft.Win32;
using Kros.TroubleShooterInput;
using System.Threading;

namespace Kros.TroubleShooterClient.Patches
{
    public class FrameworkVersionPatch : Patch
    {
        public override string Description => "Skontroluje, či máte nainštalovanú požadovanú verziu .Net Framework 4.6";

        public override string PatchName => "Verzia Frameworku";

        public override string Instruction
        {
            get
            {
                InstructionBuilder b = new InstructionBuilder("Je potrebné nainštalovať .Net Framework verziu 4.6 alebo väčšiu.");
                b.Points.Add("Vyberte požadovanú verziu .Net Frameworku zo zoznamu linkov. Postačí verzia 4.6");
                b.Points.Add("Kliknutím na link budete presmerovaný do predvoleného prehliadača, odkiaľ stiahnete inštalačný súbor.");
                b.Links.Add(".Net Framework 4.6", "https://www.microsoft.com/en-us/download/confirmation.aspx?id=48130");
                b.Links.Add(".Net Framework 4.6.1", "https://www.microsoft.com/en-us/download/confirmation.aspx?id=49981");
                b.Links.Add(".Net Framework 4.6.2", "https://www.microsoft.com/en-us/download/confirmation.aspx?id=53345");
                b.Links.Add(".Net Framework 4.7", "https://www.microsoft.com/en-us/download/confirmation.aspx?id=55170");
                b.Links.Add(".Net Framework 4.7.1", "https://www.microsoft.com/en-us/download/confirmation.aspx?id=56116");
                return b.ToString();
            }
        }

        private static string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full";

        /// <summary>
        /// skontroluje .net verziu v registroch či je menšia ako 4.6
        /// </summary>
        /// <returns></returns>
        protected override bool ComplexIdentify()
        {
            int version = (int)Registry.GetValue(key, "Release", "0");
            return version < 393295;
        }

        /// <summary>
        /// nevie opraviť ale poskytne návod na opravu
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected override bool SolveProblem(RunData data)
        {
            return false;
        }

        protected override bool FastIdentify(RunData runData)
        {
            //problem can be identified only in code way.
            return false;
        }

        protected override bool ControlProblem(RunData runData)
        {
            return !ComplexIdentify();
        }
    }
}
