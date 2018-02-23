namespace Kros.TroubleShooterCommon.Models
{
    /// <summary>
    /// Source file will be securelly warpped into this structure and sent to client
    /// </summary>
    public class ProtectedSource
    {
        /// <summary>
        /// Source code best encrypted so nobody can see content
        /// </summary>
        public byte[] SourceCode { get; set; }
        /// <summary>
        /// Public key so client can derive common secret
        /// </summary>
        public byte[] DhPublicServer { get; set; }
        /// <summary>
        /// signature so client can verify server
        /// </summary>
        public byte[] Signature { get; set; }
    }
}
