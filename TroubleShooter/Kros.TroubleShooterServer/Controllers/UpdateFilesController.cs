using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using ElipticCurves;
using Kros.TroubleShooterCommon;
using Kros.TroubleShooterCommon.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kros.TroubleShooterServer.Controllers
{
    /// <summary>
    /// provides functions needed for update sources on clients
    /// </summary>
    [Route("api/[controller]")]
    public class UpdateFilesController : Controller
    {
        /// <summary>
        /// directory with patch files
        /// </summary>
        private const string SOURCE_FILES_DIR = "UpdateFiles";

        /// <summary>
        /// server private key to sign protected source files
        /// </summary>
        private byte[] signatureKey = Convert.FromBase64String("YkOydPkgS0av3U6xeOEMl74HkbMA");

        /// <summary>
        /// provider used to sign source files
        /// </summary>
        private ECSignature signatureMaker;

        /// <summary>
        /// provider used to generate key pair 
        /// </summary>
        private ECKeysGenerator keyGen;

        /// <summary>
        /// provider to derive shared secret between client and server
        /// </summary>
        private ECDiffieHelman diffieHelman;

        /// <summary>
        /// list of actual source files and its versions
        /// </summary>
        private IEnumerable<SourceFileInfo> sourceFiles;

        /// <summary>
        /// 
        /// </summary>
        public UpdateFilesController()
        {
            ElipticCurve curve = ElipticCurve.secp160r1();
            signatureMaker = new ECSignature(curve);
            keyGen = new ECKeysGenerator(curve);
            diffieHelman = new ECDiffieHelman(curve);
            sourceFiles = SourceFileInfoBuilder.GetSourceFiles(SOURCE_FILES_DIR);
        }

        /// <summary>
        /// Gets source code encoded by common secret and signed by server
        /// </summary>
        /// <param name="request">contains filename and clients public key</param>
        /// <returns>protected source</returns>
        [HttpPost("sources")]
        public ProtectedSource Post([FromBody] ProtectedSourceRequest request)
        {
            //read source code
            string sourceCode = System.IO.File.ReadAllText(Path.Combine(SOURCE_FILES_DIR, request.FileName));
            //generate key pair and derive shared secret
            byte[] dhServerPublic;
            byte[] dhServerPrivate;
            keyGen.GenerateKeyPair(out dhServerPrivate, out dhServerPublic);
            byte[] sharedSecret = diffieHelman.SharedSecret(dhServerPrivate, request.DhClientPublic);

            //send encrypted and signed source back to client;
            //send also servers public key so client can derive common secret
            return new ProtectedSource()
            {
                SourceCode = AesHandler.EncryptStringToBytes_Aes(sourceCode, sharedSecret),
                DhPublicServer = dhServerPublic,
                Signature = signatureMaker.Signature(sourceCode, signatureKey)
            };
        }

        /// <summary>
        /// gets actual file info
        /// </summary>
        /// <returns></returns>
        [HttpGet("updateinfo")]
        public IEnumerable<SourceFileInfo> GetFileInfo()
        {
            return sourceFiles;
        }
    }
}
