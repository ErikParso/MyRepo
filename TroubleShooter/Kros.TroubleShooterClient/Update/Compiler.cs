using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterInput;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Kros.TroubleShooterClient.Update
{
    /// <summary>
    /// Compiles the patch and question files in specified folder.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// The list of supported assemblies.
        /// </summary>
        private static List<MetadataReference> supportedReferences = new List<MetadataReference>()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Compiler).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(RunData).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Component).Assembly.Location)
        };

        private const string PatchesAssembly = "Patches.dll";


        /// <summary>
        /// Compiles .cs source files located in update folder into Patches.dll assembly.
        /// If compilation is unsuccessfull, sources are damaged and dll is inapplicable so this is why i delete this.
        /// </summary>
        /// <param name="path">update folder</param>
        public static void Compile(string path)
        {
            //load sources 
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
            if (Directory.Exists(path))
            {
                foreach (string file in Directory.GetFiles(path, "*.cs"))
                {
                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
                    syntaxTrees.Add(syntaxTree);
                }
            }

            //init compilation
            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: syntaxTrees,
                references: supportedReferences,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
            //emit compilation result in assembly
            EmitResult result;
            using (var fs = new FileStream(PatchesAssembly, FileMode.OpenOrCreate))
                result = compilation.Emit(fs);
            //handle compilation error, write it in log and delete failed dll and sources
            if (!result.Success)
            {
                IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                    diagnostic.IsWarningAsError ||
                    diagnostic.Severity == DiagnosticSeverity.Error);
                //display compilation errors
                Logger.LogCompilationFail(failures);
                //delete failed dll and patches  
                if (File.Exists(PatchesAssembly))
                    File.Delete(PatchesAssembly);
                Directory.Delete(path, true);
            }
        }

        /// <summary>
        /// tries to load patches assembly compiled by <see cref="Compile"/> method.
        /// </summary>
        /// <returns>
        /// loaded assembly - successfull load
        /// null - load failed for some reason (see log)
        /// </returns>
        public static Assembly LoadPatchAssembly()
        {
            try
            {
                return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, PatchesAssembly));
            }
            catch (Exception e)
            {
                Logger.LogAssemblyLoadFail(e);
                return null;
            }
        }
    }
}