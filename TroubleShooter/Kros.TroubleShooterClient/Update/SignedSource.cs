﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kros.TroubleShooterClient.Update
{
    public class ProtectedSource
    {
        public string FileName { get; set; }
        public int Version { get; set; }
        public string SourceCode { get; set; }
        public string Signature { get; set; }
    }
}
