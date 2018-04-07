//version(2018022620)

using System;
using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class ServicePackPatch : Patch
    {
        public override string Description => "Skontroluje, či máte nainštalovaný požadovaný Service Pack 1.";

        public override string PatchName => "Service Pack";

        public override string Instruction
        {
            get
            {
                InstructionBuilder b = new InstructionBuilder("Pre správnu funkčnosť programu Olymp bežiacom na operačnom systéme Windows 7 je potrebné mať nainštalovaný Service Pack s verziou aspoň 1.");
                b.Points.Add("Po kliknutí na link budete presmerovaný na stránku, kde sa dozviete viac informácií k inštalácii balíka.");
                b.Links.Add("Service Pack 1", "https://support.microsoft.com/sk-sk/help/15090/windows-7-install-service-pack-1-sp1");
                return b.ToString();
            }
        }

        /// <summary>
        /// skontroluje verziu service packu ak je to win 7
        /// </summary>
        /// <returns></returns>
        protected override bool ComplexIdentify()
        {
            //ak ma win 7
            if (Environment.OSVersion.Platform != PlatformID.Win32NT)
                return false;
            if (Environment.OSVersion.Version.Major != 6)
                return false;
            if (Environment.OSVersion.Version.Minor != 1)
                return false;
            //ma nainstalovany service pack
            if (!string.IsNullOrEmpty(Environment.OSVersion.ServicePack))
                return false;
            //nema nainstalovany service pack
            return true;
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
