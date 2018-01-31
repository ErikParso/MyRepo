using Kros.TroubleShooterClient.Model;
using System;
using System.IO;
using System.Text;

namespace Kros.TroubleShooterClient
{
    public class Logger
    {
        public static void LogIdentifyException(Exception e, Patch patche, Mode occuredInMode)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(occuredInMode.ToString() + " on " + DateTime.Now + " patch " + patche.PatchName + "(" + patche.GetType().Name + ")");
            sb.AppendLine(e.Message);
            sb.AppendLine(e.StackTrace);
            sb.AppendLine("=========================================================");
            File.AppendAllText("TSLog.txt", sb.ToString());
        }

        public enum Mode
        {
            IDENTIFICATION, SOLVING
        }

    }

    
}
