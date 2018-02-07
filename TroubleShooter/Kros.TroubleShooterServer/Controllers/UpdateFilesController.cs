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

        private ECSignature signatureMaker;

        private ECKeysGenerator keyGen;

        private string signatureKey = "0EB5C3428BAAC0A933EBF20C8BCCA1DA0C2564E27";

        public UpdateFilesController()
        {
            ElipticCurve curve = ElipticCurve.secp160r1();
            encryptor = new ECEncryption(curve);
            signatureMaker = new ECSignature(curve);
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
                string sourceCode = System.IO.File.ReadAllText(sourceFile);
                sources.Add(new ProtectedSource()
                {
                    Version = 1,
                    FileName = Path.GetFileName(sourceFile),
                    SourceCode = encryptor.Encrypt(sourceCode, publicKey, Encoding.Unicode),
                    Signature = signatureMaker.Signature(sourceCode, signatureKey)
                });
            }
            return sources;
        }
    }
}
