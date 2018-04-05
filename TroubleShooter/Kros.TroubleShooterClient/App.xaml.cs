using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.View;
using Kros.TroubleShooterInput;
using System;
using System.Text;
using System.Windows;

namespace Kros.TroubleShooterClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            switch (TroubleShooter.Current.RunData.StartupMode)
            {
                case StartupMode.COMPLEX: new MainWindow().Show(); break;
                case StartupMode.MINIMALISTIC: RunMinimalisticUglyBox(); break;
                case StartupMode.SERVIS_ONLY: new ServisWindow().Show(); break;
            }
        }


        /// <summary>
        /// Runs troubleshooter in minimalistic mode. Finds first patch which can identify 
        /// problem using fast identify method and asks user if he wants to repair problem.
        /// If problem was successfully repaired, application exits with exitcode 0.
        /// If problem wasnt repaired .. exit with code 1
        /// </summary>
        private void RunMinimalisticUglyBox()
        {
            //find patch which identifies problem
            foreach (Patch p in TroubleShooter.Current.Patches)
                if (p.FastIdentifySafe())
                {
                    if (p.Instruction == null)
                        solving(p);
                    else
                        instruct(p);
                }
            this.Shutdown(1);
        }

        /// <summary>
        /// runs troubleshooter in solving mode
        /// </summary>
        /// <param name="p"></param>
        private void solving(Patch p)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(TroubleShooter.Current.RunData.ErrorMessage);
            sb.AppendLine();
            sb.AppendLine("Troubleshooter sa pokúsi opraviť chybu automaticky. " + p.Description);
            sb.AppendLine();
            sb.AppendLine("Prajete si vykonať automatickú opravu chyby?");
            MessageBoxResult res = MessageBox.Show(sb.ToString(), "Chyba rogramu", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (res == MessageBoxResult.Yes)
            {
                if (p.SolveProblemSafe())
                {
                    StringBuilder sb2 = new StringBuilder();
                    sb2.AppendLine("Oprava problému prebehla úspešne.");
                    if (!string.IsNullOrEmpty(p.ExecutionResult))
                    {
                        sb2.AppendLine();
                        sb2.AppendLine(p.ExecutionResult);
                    }
                    MessageBox.Show(sb2.ToString(), "Oprava chyby", MessageBoxButton.OK, MessageBoxImage.Information);
                    Shutdown(0);
                }
                else
                {
                    StringBuilder sb2 = new StringBuilder();
                    sb2.AppendLine("Problém sa nepodarilo opraviť automaticky.");
                    if (!string.IsNullOrEmpty(p.ExecutionResult))
                    {
                        sb2.AppendLine();
                        sb2.AppendLine(p.ExecutionResult);
                    }
                    MessageBox.Show(sb2.ToString(), "Chyba programu", MessageBoxButton.OK, MessageBoxImage.Error);
                    Shutdown(1);
                }
            }
            else
            {
                Shutdown(1);
            }
        }

        /// <summary>
        /// runs troubleshooter in instruct mode which will display html instructions.
        /// </summary>
        /// <param name="p"></param>
        private void instruct(Patch p)
        {
            ShutdownMode = ShutdownMode.OnExplicitShutdown;
            //problem will be not solved by troubleshooter
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(TroubleShooter.Current.RunData.ErrorMessage);
            sb.AppendLine();
            sb.AppendLine("Troubleshooter identifikoval problém. " + p.Description);
            sb.AppendLine();
            sb.AppendLine("Prajete si zobraziť návod na odstránenie problému ?");
            MessageBoxResult res = MessageBox.Show(sb.ToString(), "Chyba rogramu", MessageBoxButton.YesNo, MessageBoxImage.Error);
            if (res == MessageBoxResult.Yes)
            {
                new BrowserWindow(p.Instruction).ShowDialog();
                while (!p.ControlProblemSafe())
                {
                    StringBuilder pretrvava = new StringBuilder();
                    pretrvava.AppendLine("Problém pretrváva. Ak máte problém s použitím návodu, kontaktujte prosím podporu.");
                    pretrvava.AppendLine();
                    pretrvava.AppendLine("Prajete si zobraziť inštrukcie znovu?");
                    if (MessageBox.Show(pretrvava.ToString(), "Chyba rogramu", MessageBoxButton.YesNo,
                           MessageBoxImage.Error) == MessageBoxResult.Yes)
                        new BrowserWindow(p.Instruction).ShowDialog();
                    else Environment.Exit(1);
                }

                MessageBox.Show("Problém bol úspešne odstránený.", "Chyba rogramu", MessageBoxButton.OK, MessageBoxImage.Information);
                Environment.Exit(0);
            }
            Environment.Exit(1);
        }
    }
}
