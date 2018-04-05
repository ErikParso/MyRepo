using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;

namespace Kros.TroubleShooterClient.Model
{
    /// <summary>
    /// Logs patch execution errors in file.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Logs patch execution error in file.
        /// </summary>
        /// <param name="e">the exception</param>
        /// <param name="patche">failed patch</param>
        /// <param name="occuredInMode">patch failed when identifiing problem or fixing problem.. </param>
        public static void LogPatchException(Exception e, Patch patche, Mode occuredInMode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(occuredInMode.ToString() + " on " + DateTime.Now + " patch " + patche.PatchName + "(" + patche.GetType().Name + ")");
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);
            sb.AppendLine("=========================================================");
            File.AppendAllText("TSLog.txt", sb.ToString());
        }

        /// <summary>
        /// Functionality in which patch failed
        /// </summary>
        public enum Mode
        {
            IDENTIFICATION, SOLVING, FAST_IDENTIFICATION, CONTROL_INSTRUCTIONS_RESULT
        }

        public static void LogCompilationFail(IEnumerable<Diagnostic> failures)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("COMPILATION FAILED");
            foreach (Diagnostic d in failures)
                sb.AppendLine(d.ToString());
            sb.AppendLine("=========================================================");
            File.AppendAllText("TSLog.txt", sb.ToString());
            if (MessageBox.Show("Opravné súbory sa nepodarilo skompilovať. Viac informácií nájdete v príslušnom log súbore. Prajete si ho teraz otvoriť?",
                "Chyba kompilácie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Process.Start("TSLog.txt");
        }

        public static void LogAssemblyLoadFail(Exception e)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("PATCH ASSEMBLY LOADING FAILED");
            sb.Append(e);
            sb.AppendLine("=========================================================");
            File.AppendAllText("TSLog.txt", sb.ToString());
            if (MessageBox.Show("Nepodarilo sa načítať knižnicu s opravnými súbormi. Viac informácií nájdete v príslušnom log súbore. Prajete si ho teraz otvoriť?",
                    "Chyba kompilácie", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                Process.Start("TSLog.txt");
        }

    }


}
