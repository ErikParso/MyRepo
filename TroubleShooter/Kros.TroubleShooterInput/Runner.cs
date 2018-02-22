using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;

namespace Kros.TroubleShooterInput
{
    /// <summary>
    /// Runs troubleshooter and passess data to this process by json stored in file.
    /// MemoryMapping should be better solution. 
    /// </summary>
    public class Runner
    {
        /// <summary>
        /// The troubleShooter lazy singletton
        /// </summary>
        private static Lazy<Runner> current = new Lazy<Runner>(() => new Runner());
        public static Runner Current { get { return current.Value; } }

        public RunData RunData { get; private set; }

        /// <summary>
        /// The executable file name in troubleshooter location.
        /// </summary>
        private const string EXE_FILE = "Kros.TroubleShooterClient.exe";

        /// <summary>
        /// The input data file. <see cref="RunData"/> object will be serialised there.
        /// </summary>
        public const string INPUT_FILE_NAME = "TsInputData.json";

        /// <summary>
        /// Runs troubleshooter and passess data to it.
        /// </summary>
        private Runner()
        {
            RunData = new RunData();
        }

        /// <summary>
        /// Executes troubleshooter and passess serialised run data to this.
        /// Functional troubleshooter must be located in tsLocation directory otherwise exception is thrown
        /// </summary>
        /// <param name="tsLocation">the relative location of troubleshooter</param>
        public int RunTroubleShooter(string tsLocation, StartupMode mode)
        {
            RunData.StartupMode = mode;
            // if executable was not found, throw an exception.
            if (!File.Exists(Path.Combine(tsLocation, EXE_FILE)))
            {
                throw new FileNotFoundException($"Troubleshooter was not found at location specified: {tsLocation}.");
            }
            string serialisedData = JsonConvert.SerializeObject(RunData);
            File.WriteAllText(Path.Combine(tsLocation, INPUT_FILE_NAME), serialisedData);
            ProcessStartInfo _processStartInfo = new ProcessStartInfo();
            //set working directory so it can find important files like runData, patches, patchAssembly etc.
            _processStartInfo.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, tsLocation);
            _processStartInfo.FileName = EXE_FILE;
            _processStartInfo.Arguments = INPUT_FILE_NAME;
            Process myProcess = Process.Start(_processStartInfo);
            myProcess.WaitForExit();
            return myProcess.ExitCode;
        }

        /// <summary>
        /// Method used by troubleshooter process to load run data.
        /// run data should be located in troubleshooter directory.
        /// </summary>
        /// <param name="fileName">The file name containing run data</param>
        /// <returns></returns>
        public static RunData GetData(string fileName = INPUT_FILE_NAME)
        {
            if (!File.Exists(fileName))
                return new RunData();
            RunData runData = JsonConvert.DeserializeObject<RunData>(File.ReadAllText(fileName));
            File.Delete(fileName);
            return runData;
        }
    }
}
