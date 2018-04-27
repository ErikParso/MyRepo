using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
    /// Provides functions for client updates and services.
    /// </summary>
    [Route("api/[controller]")]
    public class TroubleshooterController : Controller
    {
        /// <summary>
        /// Directory with patch files.
        /// </summary>
        private const string SOURCE_FILES_DIR = "UpdateFiles";

        /// <summary>
        /// Directory with servis attachments.
        /// </summary>
        private const string SERVIS_DIR = "Servis";

        /// <summary>
        /// Generated server private key to sign protected source files.
        /// </summary>
        private byte[] signatureKey = Convert.FromBase64String("YkOydPkgS0av3U6xeOEMl74HkbMA");

        /// <summary>
        /// Service used to sign source files.
        /// </summary>
        private ECSignature signatureMaker;

        /// <summary>
        /// Service used to generate key pair.
        /// </summary>
        private ECKeysGenerator keyGen;

        /// <summary>
        /// Service to derive shared secret between client and server.
        /// </summary>
        private ECDiffieHelman diffieHelman;

        /// <summary>
        /// List of actual source files and its versions.
        /// </summary>
        private IEnumerable<SourceFileInfo> sourceFiles;

        /// <summary>
        /// Instance used for generating attachment directories.
        /// </summary>
        private Random rnd;

        /// <summary>
        /// Database repository. Entity dbContext.
        /// </summary>
        private ServisContext ctx;

        /// <summary>
        /// <see cref="TroubleshooterController"/>
        /// </summary>
        public TroubleshooterController(ServisContext ctx)
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
        /// Gets protected source. Code encoded by common secret and signed by server.
        /// </summary>
        /// <param name="request">Contains requested source file name and clients public key to derive encryption common secret.</param>
        /// <returns>Protected source.</returns>
        [HttpPost("source")]
        public ProtectedSource Get([FromBody] ProtectedSourceRequest request)
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
        /// Stores service information and attachments on server.
        /// </summary>
        /// <returns>Result.</returns>
        [HttpPost("servis")]
        public IActionResult Post()
        {
            IFormCollection form = Request.Form;

            if (!Directory.Exists(SERVIS_DIR))
                Directory.CreateDirectory(SERVIS_DIR);

            //process attachments
            string attachmentDir = "NO_ATTACHMENTS";
            if (form.Files.Any())
            {
                attachmentDir = Path.GetRandomFileName().Replace(".","_");
                //ctreate zip with attachments zip
                using (var fileStream = new FileStream(Path.Combine(SERVIS_DIR, attachmentDir + ".zip"), FileMode.CreateNew))
                {
                    using (var archive = new ZipArchive(fileStream, ZipArchiveMode.Create))
                    {
                        foreach (IFormFile file in form.Files)
                        {
                            var demoFile = archive.CreateEntry(file.FileName);
                            using (var entryStream = demoFile.Open())
                            {
                                file.CopyTo(entryStream);
                            }
                        }
                    }
                }
            }

            //write servis information in database
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
        /// Gets server files info for update. File names with actual versions.
        /// </summary>
        [HttpGet("updateinfo")]
        public IEnumerable<SourceFileInfo> GetFileInfo()
        {
            return sourceFiles;
        }

        /// <summary>
        /// Gets all servis informations from server.
        /// </summary>
        /// <returns></returns>
        [HttpGet("problems")]
        public IEnumerable<Servis> GetServises()
        {
            IEnumerable<Servis> ret = ctx.Servises.Include("ServisInformations").OrderBy(s => s.ReceiveDate).ToList();
            return ret;
        }

        /// <summary>
        /// Tests connection.
        /// </summary>
        [HttpGet("test")]
        public IActionResult TestConn()
        {
            return Ok("server fine");
        }

        /// <summary>
        /// Clears db and attachments. 
        /// </summary>
        [HttpDelete("clear")]
        public IActionResult Clear()
        {
            if (Directory.Exists(SERVIS_DIR))
                Directory.Delete(SERVIS_DIR, true);
            ctx.Database.ExecuteSqlCommand("delete from [ServisInformation]");
            ctx.Database.ExecuteSqlCommand("delete from [Servis]");
            return Ok("database and attachments cleared");
        }

        /// <summary>
        /// Gets instruction file of number specified. 
        /// </summary>
        [HttpGet("faq/{id}")]
        public IActionResult Faq(int id)
        {
            string faqFile = $@"FAQ\{id}.pdf";
            if (!System.IO.File.Exists(faqFile))
                return BadRequest("Faq file with number specified was not found");
            else
                return new FileContentResult(System.IO.File.ReadAllBytes($@"FAQ\{id}.pdf"), "application/pdf");
        }

        /// <summary>
        /// Gets zipped attachmets. 
        /// </summary>
        [HttpGet("attachment/{name}")]
        public IActionResult GetAttachment(string name)
        {
            string attachment = $@"{SERVIS_DIR}\{name}.zip";
            if (!System.IO.File.Exists(attachment))
                return BadRequest("Attachments could not be found.");
            else
                return new FileContentResult(System.IO.File.ReadAllBytes(attachment), "application/zip");
        }
    }
}
