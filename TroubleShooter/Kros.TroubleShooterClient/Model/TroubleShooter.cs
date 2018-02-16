using Kros.TroubleShooterClient.Update;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace Kros.TroubleShooterClient.Model
{
    public class TroubleShooter
    {
        /// <summary>
        /// The troubleShooter lazy sibgletton
        /// </summary>
        private static Lazy<TroubleShooter> current = new Lazy<TroubleShooter>(() => new TroubleShooter());
        public static TroubleShooter Current { get { return current.Value; } }

        /// <summary>
        /// Location of patches to compile
        /// </summary>
        private string SOURCES_LOCATION = "Patches";

        /// <summary>
        /// error message
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// List of all patches
        /// </summary>
        public List<Patch> Patches { get; }

        /// <summary>
        /// the root question for form mode.
        /// </summary>
        public Question RootQuestion { get; }

        /// <summary>
        /// Determines if server is active
        /// </summary>
        public bool ServerOnline { get; private set; }

        /// <summary>
        /// determines if its a new version of troubleshooter
        /// </summary>
        public bool IsNewVersion { get; private set; }

        /// <summary>
        /// Tries to compile patches in defined folder.
        /// Creates its instances and builds the problem structure.
        /// </summary>
        private TroubleShooter()
        {
            if (!Directory.Exists(SOURCES_LOCATION))
                Directory.CreateDirectory(SOURCES_LOCATION);
            //get latest source files
            Updater updater = new Updater(SOURCES_LOCATION);
            ServerOnline = updater.TryConnection();
            IsNewVersion = updater.Execute();
            //try compile assemblies
            Assembly patchAssembly = Compiler.Compile(SOURCES_LOCATION, IsNewVersion);
            if (patchAssembly == null)
                throw new Exception("patch compilation failed");
            //register all patches
            Patches = new List<Patch>();
            var compiledPatches = (from type in patchAssembly.GetTypes()
                                   where type.IsSubclassOf(typeof(Patch))
                                   select (Patch)Activator.CreateInstance(type)).ToList();
            foreach (Patch patch in compiledPatches)
                Patches.Add(patch);
            RootQuestion = (from type in patchAssembly.GetTypes()
                            where type.IsSubclassOf(typeof(Question))
                            where type.GetCustomAttributes(typeof(RootQuestionAttribute)).Count() != 0
                            select (Question)Activator.CreateInstance(type)).FirstOrDefault();
        }

        /// <summary>
        /// TroubleShooter tries solve exception
        /// <see cref="ProblemOccurred(Problem)"/>
        /// </summary>
        public void Fire(Exception ex)
        {
            Exception = ex;
            if (App.Current == null)
            {
                new App();
                App.Current.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            }
            new MainWindow().ShowDialog();
        }
    }
}
