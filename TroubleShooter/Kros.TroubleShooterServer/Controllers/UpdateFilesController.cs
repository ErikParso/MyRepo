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
    [Route("api/[controller]")]
    public class UpdateFilesController : Controller
    {
        private const string SOURCE_FILES_DIR = "UpdateFiles";
        private const string signatureKey = "0EB5C3428BAAC0A933EBF20C8BCCA1DA0C2564E27";

        private ECEncryption encryptor;

        private ECSignature signatureMaker;

        private ECKeysGenerator keyGen;

        private ECDiffieHelman diffieHelman;

        private IEnumerable<SourceFileInfo> sourceFiles;

        public UpdateFilesController()
        {
            ElipticCurve curve = ElipticCurve.secp160r1();
            encryptor = new ECEncryption(curve);
            signatureMaker = new ECSignature(curve);
            keyGen = new ECKeysGenerator(curve);
            diffieHelman = new ECDiffieHelman(curve);
            sourceFiles = SourceFileInfoBuilder.GetSourceFiles(SOURCE_FILES_DIR);
        }

        /// <summary>
        /// Gets source code encoded using clients publicKey 
        /// signed by server
        /// </summary>
        /// <param name="dhClientPublic">clients public key</param>
        /// <returns></returns>
        [HttpGet("sources")]
        public ProtectedSource Get(string dhClientPublic, string sourceFile)
        {
            string sourceCode = sourceFile == null ? 
                "source code should be AES encrypted so nobody can see sensitive data" :
                System.IO.File.ReadAllText(Path.Combine(SOURCE_FILES_DIR, sourceFile));
            
            string dhServerPublic, dhServerPrivate;
            keyGen.GenerateKeyPair(out dhServerPrivate, out dhServerPublic);
            string sharedSecret = diffieHelman.SharedSecret(dhServerPrivate, dhClientPublic);
            if (sharedSecret == null)
                sharedSecret = "0";

            return new ProtectedSource()
            {
                //SourceCode = encryptor.Encrypt(sourceCode, dhClientPublic, Encoding.Unicode),
                SourceCode = AesGenerator.EncryptStringToBytes_Aes(sourceCode, sharedSecret),
                DhPublicServer = dhServerPublic,
                Signature = signatureMaker.Signature(sourceCode, signatureKey)
            };
        }

        [HttpGet("updateinfo")]
        public IEnumerable<SourceFileInfo> GetFileInfo()
        {
            return sourceFiles;
        }
    }
}
