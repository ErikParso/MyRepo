using System;
using System.Collections.Generic;

namespace Kros.TroubleShooterCommon.Models
{
    public class SourceFileInfo
    {
        public string FileName { get; set; }

        /// <summary>
        /// if null file is removed
        /// </summary>
        public int Version { get; set; }
    }
}
