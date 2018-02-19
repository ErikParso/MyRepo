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
        /// The relative path containing troubleshooter executable.
        /// Run data file will be stored here as well.
        /// </summary>
        private string tsLocation;

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
        /// <param name="tsLocation">
        /// the relative path containing troubleshooter
        /// </param>
        public Runner(string tsLocation)
        {
            // if executable was not found, throw an exception.
            if (!File.Exists(Path.Combine(tsLocation, EXE_FILE)))
            {
                throw new FileNotFoundException($"Troubleshooter was not found at location specified: {tsLocation}.");
            }
            this.tsLocation = tsLocation;
        }

        /// <summary>
        /// Executes troubleshooter and passess serialised run data to this.
        /// Functional troubleshooter must be located in tsLocation directory, this is validated by ctor.
        /// </summary>
        /// <param name="data">
        /// data to be passed to troubleshooter
        /// </param>
        public void RunTroubleShooter(RunData data)
        {
            string serialisedData = JsonConvert.SerializeObject(data);
            File.WriteAllText(Path.Combine(tsLocation, INPUT_FILE_NAME), serialisedData);
            ProcessStartInfo _processStartInfo = new ProcessStartInfo();
            //set working directory so it can find important files like runData, patches, patchAssembly etc.
            _processStartInfo.WorkingDirectory = Path.Combine(Environment.CurrentDirectory, tsLocation);
            _processStartInfo.FileName = EXE_FILE;
            _processStartInfo.Arguments = INPUT_FILE_NAME;
            Process myProcess = Process.Start(_processStartInfo);
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
                return null;
            return JsonConvert.DeserializeObject<RunData>(File.ReadAllText(fileName));
        }
    }
}
