using Kros.TroubleShooterClient.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

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
        public string Error { get; set; }

        /// <summary>
        /// List of all patches
        /// </summary>
        public List<Patch> Patches { get; }

        /// <summary>
        /// the root question for form mode.
        /// </summary>
        public Question RootQuestion { get; }

        /// <summary>
        /// Tries to compile patches in defined folder.
        /// Creates its instances and builds the problem structure.
        /// </summary>
        private TroubleShooter()
        {
            //get latest source files
            bool newVersion = new Updater(SOURCES_LOCATION).Execute();
            //try compile assemblies
            Assembly patchAssembly = Compiler.Compile(SOURCES_LOCATION, newVersion);
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
        /// TroubleShooter will catch all unhandled exceptions and executes handling method.
        /// <see cref="ProblemOccurred(Problem)"/>
        /// </summary>
        public void Listen()
        {
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += (sender, args) =>
            {
                throw new NotImplementedException();
            };
        }


    }
}
