//version(2017021907)
using Kros.TroubleShooterClient.Model;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Threading;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class p2017102600 : Patch
    {
        public override string Description { get { return "Došlo k poškodeniu registrov uchovávajúcich názvy súborov s fontami. Problém sa môže prejavovať pri tlači."; } }

        public override string PatchName { get { return "Poškodené fonty"; } }

        public override string HtmlInfo { get { return null; } }

        public override int SolvesProblem { get { return 2017121503; } }

        private static string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts";

        protected override bool IdentifyProblem(RunData e)
        {
            Thread.Sleep(100);
            return !Registry.GetValue(key, "Arial Narrow CE (TrueType)", "not registered").Equals("arince.ttf")
                || !Registry.GetValue(key, "Arial Narrow CE Bold (TrueType)", "not registered").Equals("arinceb.ttf")
                || !Registry.GetValue(key, "Arial CE Bold (TrueType)", "not registered").Equals("CEARIABD.TTF");
        }

        protected override bool SolveProblem()
        {
            Thread.Sleep(100);
            Registry.SetValue(key, "Arial Narrow CE (TrueType)", "arince.ttf");
            Registry.SetValue(key, "Arial Narrow CE Bold (TrueType)", "arinceb.ttf");
            Registry.SetValue(key, "Arial CE Bold (TrueType)", "CEARIABD.TTF");
            return true;
        }
    }
}
