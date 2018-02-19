using Kros.TroubleShooterInput;
using System;
using System.Collections.Generic;
using System.IO;

namespace HostingApp
{
    class Program
    {
        static void Main(string[] args)
        {
            new Runner("TroubleShooter").RunTroubleShooter(new RunData()
            {
                Exception = new FileNotFoundException("testing exception"),
                Data = new Dictionary<string, string>()
                {
                    { "database", "path/to/database"},
                    { "olymp" , "path/to/olymp"}
                }

            });
        }
    }
}
