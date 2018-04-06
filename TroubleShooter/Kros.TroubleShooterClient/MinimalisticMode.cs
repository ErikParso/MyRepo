using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.View;
using Kros.TroubleShooterClient.ViewModel;
using System.Text;
using System.Windows;

namespace Kros.TroubleShooterClient
{
    public class MinimalisticMode
    {
        /// <summary>
        /// Runs troubleshooter in minimalistic mode. Finds first patch which can identify 
        /// problem using fast identify method and asks user if he wants to repair problem.
        /// If problem was successfully repaired, application exits with exitcode 0.
        /// If problem wasnt repaired .. exit with code 1
        /// </summary>
        public bool Run()
        {
            StringBuilder sb = new StringBuilder();

            //find patch which identifies problem
            foreach (Patch p in TroubleShooter.Current.Patches)
            {
                if (p.FastIdentifySafe())
                {
                    sb.AppendLine(TroubleShooter.Current.RunData.ErrorMessage);
                    sb.AppendLine();
                    sb.AppendLine("Troubleshooter sa pokúsi opraviť chybu automaticky. " + p.Description);
                    sb.AppendLine();
                    sb.AppendLine("Prajete si vykonať automatickú opravu chyby?");
                    MessageBoxResult res = MessageBox.Show(sb.ToString(), "Chyba rogramu", MessageBoxButton.YesNo, MessageBoxImage.Error);
                    sb.Clear();
                    if (res == MessageBoxResult.Yes)
                    {
                        if (p.SolveProblemSafe())
                        {
                            sb.AppendLine("Automatická prava problému prebehla úspešne.");
                            MessageBox.Show(sb.ToString(), "Oprava chyby", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else if (p.Instruction != null)
                        {
                            sb.AppendLine("Problém sa nepodarilo opraviť automaticky. K dispozícii je návod na jeho odstránenie. Prajete si ho zobraziť?");
                            if (MessageBox.Show(sb.ToString(), "Chyba programu", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                            {
                                BrowserWindow browser = new BrowserWindow(new PatchResultVM(p));
                                browser.ShowDialog();
                                if (browser.Success)
                                {
                                    MessageBox.Show("Problém bol úspešne odstránený.", "Chyba rogramu", MessageBoxButton.OK, MessageBoxImage.Information);
                                    return true;
                                }
                            }
                        }
                    }
                    break;
                }
            }
            MessageBox.Show("Problém sa nepodarilo odstrániť.", "Chyba rogramu", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }
    }
}
