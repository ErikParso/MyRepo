using System;
using System.Collections.Generic;
using System.Text;

namespace Kros.TroubleShooterCommon.Models
{
    public class ProtectedSource
    {
        public byte[] SourceCode { get; set; }
        public string DhPublicServer { get; set; }
        public string Signature { get; set; }
    }
}
