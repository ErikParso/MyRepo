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


        /// <summary>
        /// Compiles .cs source files located in update folder
        /// </summary>
        /// <param name="path">update folder</param>
        /// <param name="recompile">if its a new version</param>
        /// <returns></returns>
        public static Assembly Compile(string path, bool recompile)
        {
            //if version changed we need to recompile files.
            if (recompile)
            {
                List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();
                if (Directory.Exists(path))
                {
                    foreach (string file in Directory.GetFiles(path, "*.cs"))
                    {
                        SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(file));
                        syntaxTrees.Add(syntaxTree);
                    }
                }

                CSharpCompilation compilation = CSharpCompilation.Create(
                    Path.GetRandomFileName(),
                    syntaxTrees: syntaxTrees,
                    references: supportedReferences,
                    options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

                //emit compilation result in assembly
                EmitResult result;
                using (var fs = new FileStream("Patches.dll", FileMode.OpenOrCreate))
                    result = compilation.Emit(fs);
                //handle compilation error
                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);
                    //display compilation errors
                    Logger.LogCompilationFail(failures);
                    //delete failed dll and patches  
                    if (File.Exists("Patches.dll"))
                        File.Delete("Patches.dll");
                    Directory.Delete(path, true);
                }
            }
            //load patches and questions assembly
            if (!File.Exists("Patches.dll"))
                return null;
            else
                return Assembly.LoadFile(Path.Combine(Environment.CurrentDirectory, "Patches.dll"));
        }
    }
}
