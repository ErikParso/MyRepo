using System;
using System.IO;
using System.Text;

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
        public static void LogIdentifyException(Exception e, Patch patche, Mode occuredInMode)
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
            IDENTIFICATION, SOLVING
        }

    }

    
}
