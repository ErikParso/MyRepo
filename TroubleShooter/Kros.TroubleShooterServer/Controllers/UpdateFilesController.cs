﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using ElipticCurves;
using Kros.TroubleShooterCommon;
using Kros.TroubleShooterCommon.Models;
using Microsoft.ApplicationInsights.Extensibility.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;

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
        /// directory with patch files
        /// </summary>
        private const string SERVIS_DIR = "Servis";

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

        [HttpPost("service")]
        public IActionResult Post()
        {
            if (!Directory.Exists(SERVIS_DIR))
                Directory.CreateDirectory(SERVIS_DIR);
            IFormCollection form  = Request.Form;
            foreach (IFormFile file in form.Files)
            {
                using (FileStream fs = new FileStream(Path.Combine(SERVIS_DIR, file.FileName), FileMode.OpenOrCreate))
                {
                    file.CopyTo(fs);
                }
            }
            foreach (string key in form.Keys)
            {
                StringValues val;
                form.TryGetValue(key, out val);
                Debug.WriteLine(key + " -");
                Debug.WriteLine("\t" + val);
            }
            return Ok();
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
