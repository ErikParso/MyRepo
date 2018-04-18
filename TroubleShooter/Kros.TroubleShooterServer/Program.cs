using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Kros.TroubleShooterServer
{
    /// <summary>
    /// The troubleshooter server hosting.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main method.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        /// <summary>
        /// Builds wbhost.
        /// </summary>
        /// <param name="args">Arguments</param>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
