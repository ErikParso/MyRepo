﻿//version(2018022203)
using Kros.TroubleShooterClient.Model;
using Microsoft.Win32;
using System.Threading;
using Kros.TroubleShooterInput;

namespace Kros.TroubleShooterClient.Patches
{
    public class PoskodeneFontyPatch : Patch
    {
        public override string Description { get { return "Došlo k poškodeniu registrov uchovávajúcich názvy súborov s fontami. Problém sa môže prejavovať pri tlači."; } }

        public override string PatchName { get { return "Poškodené fonty"; } }

        public override string HtmlInfo { get { return null; } }

        private static string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts";

        protected override bool ComplexIdentify()
        {
            Thread.Sleep(100);
            return !Registry.GetValue(key, "Arial Narrow CE (TrueType)", "not registered").Equals("arince.ttf")
                || !Registry.GetValue(key, "Arial Narrow CE Bold (TrueType)", "not registered").Equals("arinceb.ttf")
                || !Registry.GetValue(key, "Arial CE Bold (TrueType)", "not registered").Equals("CEARIABD.TTF");
        }

        protected override bool SolveProblem(RunData data)
        {
            Thread.Sleep(100);
            Registry.SetValue(key, "Arial Narrow CE (TrueType)", "arince.ttf");
            Registry.SetValue(key, "Arial Narrow CE Bold (TrueType)", "arinceb.ttf");
            Registry.SetValue(key, "Arial CE Bold (TrueType)", "CEARIABD.TTF");
            return true;
        }

        protected override bool FastIdentify(RunData runData)
        {
            //problem can be identified only in code way.
            return false;
        }
    }
}
