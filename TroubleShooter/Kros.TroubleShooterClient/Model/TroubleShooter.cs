using Kros.TroubleShooterClient.Update;
using Kros.TroubleShooterInput;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Kros.TroubleShooterClient.Model
{
    /// <summary>
    /// The data context of the application. Contains patches, problem tree and input data to easily define problem.
    /// </summary>
    public class TroubleShooter
    {
        /// <summary>
        /// The troubleShooter lazy singletton
        /// </summary>
        private static Lazy<TroubleShooter> current = new Lazy<TroubleShooter>(() => new TroubleShooter());
        public static TroubleShooter Current { get { return current.Value; } }

        /// <summary>
        /// Location of patches and Questions to compile.
        /// After successfull update an source files will be located there.
        /// </summary>
        private string SOURCES_LOCATION = "Patches";

        /// <summary>
        /// troubleshooter startup data. Will be deserialised from
        /// <see cref="Runner.INPUT_FILE_NAME"/> file
        /// </summary>
        public RunData RunData { get; private set; }

        /// <summary>
        /// Collection of all compiled patches
        /// </summary>
        public List<Patch> Patches { get; }

        /// <summary>
        /// The compiled Question tree structure
        /// </summary>
        public Question RootQuestion { get; }

        /// <summary>
        /// Determines if server is active.
        /// If false - troubleshooter cannot update
        ///          - data cannot be send for servis
        /// </summary>
        public bool ServerOnline { get; private set; }

        /// <summary>
        /// determines if its a new version of troubleshooter files.
        /// If yes, recompilation is needed. 
        /// </summary>
        public bool IsNewVersion { get; private set; }

        /// <summary>
        /// Client used to communicate with web api.
        /// </summary>
        public TroubleShooterClient Client { get; private set; }

        /// <summary>
        /// - gets a new version of patches and questions
        /// - tries to compile patches and questions if update was successfull
        ///   otherwise the latest compiled dll is used "Patches.dll"
        /// - creates patches instances and builds problem tree.
        /// </summary>
        private TroubleShooter()
        {
            Client = new TroubleShooterClient();
            //create update dir
            if (!Directory.Exists(SOURCES_LOCATION))
                Directory.CreateDirectory(SOURCES_LOCATION);
            //get latest source files if server is online... check if its a new version
            Updater updater = new Updater(SOURCES_LOCATION, Client);
            ServerOnline = Client.TryConnection();
            IsNewVersion = ServerOnline ? updater.Execute() : false;
            //compile if its a new version othervise use last time compiled assembly
            Assembly patchAssembly = Compiler.Compile(SOURCES_LOCATION, IsNewVersion);
            if (patchAssembly == null)
                throw new Exception("patch compilation failed");
            //register all patches, create its instances
            Patches = new List<Patch>();
            var compiledPatches = (from type in patchAssembly.GetTypes()
                                   where type.IsSubclassOf(typeof(Patch))
                                   select (Patch)Activator.CreateInstance(type)).ToList();
            foreach (Patch patch in compiledPatches)
                Patches.Add(patch);
            //initialise a root question -  decorated by RootQuestionAttribute
            RootQuestion = (from type in patchAssembly.GetTypes()
                            where type.IsSubclassOf(typeof(Question))
                            where type.GetCustomAttributes(typeof(RootQuestionAttribute)).Count() != 0
                            select (Question)Activator.CreateInstance(type)).FirstOrDefault();
            //load run data if process started using Kros.TroubleShooterInputs method
            RunData = Runner.GetData();
        }
    }
}
