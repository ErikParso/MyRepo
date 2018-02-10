using System;
using System.Collections.Generic;
using System.Text;

namespace Kros.TroubleShooterCommon.Models
{
    public class ProtectedSource
    {
        public byte[] SourceCode { get; set; }
        public byte[] DhPublicServer { get; set; }
        public byte[] Signature { get; set; }
    }
}
