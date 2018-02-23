namespace Kros.TroubleShooterCommon.Models
{
    /// <summary>
    /// clients request for protected file to server
    /// </summary>
    public class ProtectedSourceRequest
    {
        /// <summary>
        /// the file name
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// clients public so server can derive common secret
        /// </summary>
        public byte[] DhClientPublic { get; set; }
    }
}
