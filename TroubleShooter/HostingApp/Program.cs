using System;
using Kros.TroubleShooterClient.Model;

namespace HostingApp
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            TroubleShooter.Current.Fire(null);
            Console.ReadLine();
            TroubleShooter.Current.Fire(new Exception("exception test"));
            Console.WriteLine("problem fixed");
            Console.ReadLine();
        }
    }
}
