using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Kros.TroubleShooterClient.Model
{
    /// <summary>
    /// Compiles the patch files in specified folder.
    /// </summary>
    public class Compiler
    {
        /// <summary>
        /// The list of supported assemblies.
        /// </summary>
        private static List<MetadataReference> supportedReferences = new List<MetadataReference>()
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(Compiler).Assembly.Location)
        };

        /// <summary>
        /// Compiles all patches in specified folder.
        /// </summary>
        /// <returns>
        /// Compiled Assembly.
        /// </returns>
        public static Assembly Compile(string path)
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

            Assembly compiledAssembly;
            using (var ms = new MemoryStream())
            {
                EmitResult result = compilation.Emit(ms);

                if (!result.Success)
                {
                    IEnumerable<Diagnostic> failures = result.Diagnostics.Where(diagnostic =>
                        diagnostic.IsWarningAsError ||
                        diagnostic.Severity == DiagnosticSeverity.Error);

                    foreach (Diagnostic diagnostic in failures)
                    {
                        Console.Error.WriteLine("{0}: {1}", diagnostic.Id, diagnostic.GetMessage());
                    }

                    compiledAssembly = null;
                }
                else
                {
                    ms.Seek(0, SeekOrigin.Begin);
                    compiledAssembly = Assembly.Load(ms.ToArray());
                }
            }
            return compiledAssembly;
        }
    }
}
