using Kros.TroubleShooterCommon.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Kros.TroubleShooterServer
{
    /// <summary>
    /// Takes version info from source file.
    /// </summary>
    public class SourceFileInfoBuilder
    {
        private static Regex versionRegex = new Regex(@"^\/\/version\((?'version'\d{10})\)");

        /// <summary>
        /// Reads source files and returns info collection (name and version).
        /// </summary>
        /// <param name="sourceFilesDirectory">Source files path.</param>
        /// <returns>Source file info collection.</returns>
        public static IEnumerable<SourceFileInfo> GetSourceFiles(string sourceFilesDirectory)
        {
            foreach (string sourceFile in Directory.GetFiles(sourceFilesDirectory, "*.cs"))
            {
                SourceFileInfo info = new SourceFileInfo();
                string sourceCode = System.IO.File.ReadAllText(sourceFile);
                info.FileName = Path.GetFileName(sourceFile);
                info.Version = GetVersionFromSource(sourceCode, info.FileName);
                yield return info;
            }
        }

        private static int GetVersionFromSource(string source, string fileName)
        {
            Match m = versionRegex.Match(source);
            if (m.Success)
            {
                return int.Parse(m.Groups["version"].Value);
            }
            else
            {
                throw new Exception($@"File {fileName} does not contain version information. Add //version(YYYYMMDDxx) comment at the start of the source file.");
            }
        }
    }
}
