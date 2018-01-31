using BattleshipsDatabase;
using BattleshipsServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsServer
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost selfHost = new ServiceHost(typeof(BattleshipsService));
            try
            {
                selfHost.Open();
                Console.WriteLine("The service is ready. Press <ENTER> to terminate service.");
                Console.ReadLine();
                selfHost.Close();
            }
            catch (CommunicationException ce)
            {
                Console.WriteLine("An exception occurred: {0}", ce.Message);
                selfHost.Abort();
            }

        }
    }
}
