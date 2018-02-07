using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ElipticCurves;
using Kros.TroubleShooterServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kros.TroubleShooterServer.Controllers
{
    [Route("api/[controller]")]
    public class UpdateFilesController : Controller
    {
        private ECEncryption encryptor;

        private ECSignature signature;

        private ECKeysGenerator keyGen;

        public UpdateFilesController()
        {
            ElipticCurve curve = ElipticCurve.secp160r1();
            encryptor = new ECEncryption(curve);
            signature = new ECSignature(curve);
            keyGen = new ECKeysGenerator(curve);
        }

        /// <summary>
        /// Gets source code encoded using clients publicKey 
        /// signed by server
        /// </summary>
        /// <param name="publicKey">clients public key</param>
        /// <returns></returns>
        [HttpGet("protected")]
        public IEnumerable<ProtectedSource> Get(string publicKey)
        {
            List<ProtectedSource> sources = new List<ProtectedSource>();
            foreach (string sourceFile in Directory.GetFiles("UpdateFiles", "*.cs"))
            {
                sources.Add(new ProtectedSource()
                {
                    Version = 1,
                    FileName = Path.GetFileName(sourceFile),
                    SourceCode = encryptor.Encrypt(System.IO.File.ReadAllText(sourceFile), publicKey, Encoding.Unicode),
                    Signature = new Random().Next().ToString()
                });
            }
            return sources;
        }
    }
}
