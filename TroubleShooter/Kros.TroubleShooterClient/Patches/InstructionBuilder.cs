//version(2018022619)
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kros.TroubleShooterClient.Patches
{
    public class InstructionBuilder
    {
        public string MainText { get; }

        public List<string> Points { get; }

        public Dictionary<string, string> Links { get; }

        public bool NumberedPoints { get; set; } = false;

        public InstructionBuilder(string mainText = null)
        {
            MainText = mainText;
            Points = new List<string>();
            Links = new Dictionary<string, string>();
        }

        public void AdFaqLik(string title, int faqNum)
        {
            Links.Add(title, Path.Combine(TroubleShooterClient.URI, TroubleShooterClient.SERVICE_PATH, "faq", faqNum.ToString()));
        }

        public override string ToString()
        {     
            StringBuilder sb = new StringBuilder("<head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'/></head>");
            sb.AppendLine("<font face='Segoe UI' size='2'>");
            if (MainText != null)
                sb.AppendLine($"<p>{MainText}</p>");
            if (Points.Count != 0)
            {
                sb.AppendLine(NumberedPoints ? "<ol>" : "<ul>");
                foreach (string point in Points)
                    sb.AppendLine($"<li>{point}</li>");
                sb.AppendLine(NumberedPoints ? "</ol>" : "</ul>");
            }
            if (Links.Count != 0)
            {
                sb.AppendLine("<p>Užitočné linky:</p>");
                sb.AppendLine("<ul>");
                foreach (KeyValuePair<string,string> link in Links)
                    sb.AppendLine($"<li><a target='_blank' href='{link.Value}'>{link.Key}</a></li>");
                sb.AppendLine("</ul>");
            }
            sb.AppendLine("</font>");
            return sb.ToString();
        }
    }
}
