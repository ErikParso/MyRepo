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
        private void RunMinimalisticNiceBox()
        {
            //find patch which identifies problem
            foreach (Patch p in TroubleShooter.Current.Patches)
            {
                if (p.FastIdentifySafe())
                {
                    //display minimalistic window
                    MiniWindow w = new MiniWindow(p);
                    w.ShowDialog();
                    if (w.ProblemFixed == MiniWindow.Result.Success)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Oprava problému prebehla úspešne.");
                        if (!string.IsNullOrEmpty(p.ExecutionResult))
                        {
                            sb.AppendLine();
                            sb.AppendLine(p.ExecutionResult);
                        }
                        MessageBox.Show(sb.ToString(), "Oprava chyby", MessageBoxButton.OK, MessageBoxImage.Information);
                        Environment.ExitCode = 0;
                        return;
                    }
                    else if (w.ProblemFixed == MiniWindow.Result.Fail)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Problém sa nepodarilo opraviť automaticky.");
                        if (!string.IsNullOrEmpty(p.ExecutionResult))
                        {
                            sb.AppendLine();
                            sb.AppendLine(p.ExecutionResult);
                        }
                        MessageBox.Show(sb.ToString(), "Chyba programu", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            Environment.ExitCode = 1;
            this.Shutdown();
        }

        /// <summary>
        /// to iste co <see cref="RunMinimalisticNiceBox"/> iba v skaredom messageboxe lebo je to prehladnejsie :D 
        /// </summary>
        private void RunMinimalisticUglyBox()
        {
            //find patch which identifies problem
            foreach (Patch p in TroubleShooter.Current.Patches)
            {
                if (p.FastIdentifySafe())
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
            }
            this.Shutdown(1);
        }

    }
}
