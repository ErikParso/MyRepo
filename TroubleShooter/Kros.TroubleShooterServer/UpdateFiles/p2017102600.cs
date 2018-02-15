//version(2017020803)

using Kros.TroubleShooterClient.Model;
using Microsoft.Win32;
using System.Threading;

namespace Kros.TroubleShooterClient.Patches
{
    public class p2017102600 : Patch
    {
        private static string key = @"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts";

        public override string Description { get { return "XDošlo k poškodeniu registrov uchovávajúcich názvy súborov s fontami. Problém sa môže prejavovať pri tlači."; } }

        public override string PatchName { get { return "p2017102600"; } }

        public override string HtmlInfo { get { return "<ul><li>point 1</li><li><span style=\"color: red; \">This is important text!</span></li><li><strong>point 3</strong></li></ul>"; } }

        public override int SolvesProblem { get { return 2017121503; } }

        protected override bool IdentifyProblem()
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

    public class p2017102601 : Patch
    {
        public override string Description { get { return "utomaticky identifikuje problem XY ale nevie ho opravit"; } }

        public override string PatchName { get { return "p2017102601"; } }

        public override int SolvesProblem { get { return 0; } }

        public override string HtmlInfo { get { return "some text"; } }

        protected override bool IdentifyProblem()
        {
            Thread.Sleep(100);
            return true;
        }

        protected override bool SolveProblem()
        {
            Thread.Sleep(100);
            return false;
        }
    }

    public class p2017102602 : Patch
    {
        public override string Description { get { return "utomaticky identifikuje problem XY a vie ho opravit"; } }

        public override string PatchName { get { return "p2017102602"; } }

        public override string HtmlInfo { get { return null; } }

        public override int SolvesProblem { get { return 0; } }

        protected override bool IdentifyProblem()
        {
            Thread.Sleep(100);
            return true;
        }

        protected override bool SolveProblem()
        {
            Thread.Sleep(100);
            return true;
        }
    }
}
