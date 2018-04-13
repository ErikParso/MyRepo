using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ElipticCurves;
using Kros.TroubleShooterCommon;
using Kros.TroubleShooterCommon.Models;
using Kros.TroubleShooterServer.Database;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;

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
        /// for generating attachment directories
        /// </summary>
        private Random rnd;

        /// <summary>
        /// servis database entity model
        /// </summary>
        private ServisContext ctx;

        /// <summary>
        /// see calss info
        /// </summary>
        public UpdateFilesController(ServisContext ctx)
        {
            this.ctx = ctx;
            ElipticCurve curve = ElipticCurve.secp160r1();
            signatureMaker = new ECSignature(curve);
            keyGen = new ECKeysGenerator(curve);
            diffieHelman = new ECDiffieHelman(curve);
            sourceFiles = SourceFileInfoBuilder.GetSourceFiles(SOURCE_FILES_DIR);
            rnd = new Random();
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

            string attachmentDir = null;
            while (string.IsNullOrEmpty(attachmentDir) || Directory.Exists(Path.Combine(SERVIS_DIR, attachmentDir)))
            {
                byte[] dirnamebytes = new byte[30];
                rnd.NextBytes(dirnamebytes);
                attachmentDir = Convert.ToBase64String(dirnamebytes);
                foreach (char c in Path.GetInvalidFileNameChars())
                    attachmentDir = attachmentDir.Replace(c, '_');
            }
            Directory.CreateDirectory(Path.Combine(SERVIS_DIR, attachmentDir));

            IFormCollection form = Request.Form;
            foreach (IFormFile file in form.Files)
            {
                using (FileStream fs = new FileStream(Path.Combine(SERVIS_DIR, attachmentDir, file.FileName), FileMode.OpenOrCreate))
                {
                    file.CopyTo(fs);
                }
            }

            Servis s = new Servis()
            {
                AttachmentsDirectory = attachmentDir,
                ServisInformations = new List<ServisInformation>(),
                ReceiveDate = DateTime.Now,
            };

            foreach (string key in form.Keys)
            {
                StringValues val;
                form.TryGetValue(key, out val);
                ServisInformation inf = new ServisInformation()
                {
                    Title = key,
                    Value = val.ToString(),
                    Servis = s
                };
                ctx.ServisInformations.Add(inf);
            }
            ctx.SaveChanges();

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

        [HttpGet("errors")]
        public IEnumerable<Servis> GetServises()
        {
            IEnumerable<Servis> ret = ctx.Servises.Include("ServisInformations").OrderBy(s => s.ReceiveDate).ToList();
            return ret;
        }

        /// <summary>
        /// gets actual file info
        /// </summary>
        /// <returns></returns>
        [HttpGet("test")]
        public IActionResult TestConn()
        {
            return Ok("server fine");
        }

        /// <summary>
        /// Clears db and attachments. 
        /// </summary>
        /// <returns></returns>
        [HttpGet("clear")]
        public IActionResult Clear()
        {
            if (Directory.Exists(SERVIS_DIR))
                Directory.Delete(SERVIS_DIR, true);
            ctx.Database.ExecuteSqlCommand("delete from [ServisInformation]");
            ctx.Database.ExecuteSqlCommand("delete from [Servis]");
            return Ok("database and attachments cleared");
        }

        /// <summary>
        /// gets actual file info
        /// </summary>
        /// <returns></returns>
        [HttpGet("faq/{id}")]
        public IActionResult TestConn(int id)
        {
            string faqFile = $@"FAQ\{id}.pdf";
            if (!System.IO.File.Exists(faqFile))
                return BadRequest("Faq file with number specified was not found");
            else
                return new FileContentResult(System.IO.File.ReadAllBytes($@"FAQ\{id}.pdf"), "application/pdf");
        }
    }
}
