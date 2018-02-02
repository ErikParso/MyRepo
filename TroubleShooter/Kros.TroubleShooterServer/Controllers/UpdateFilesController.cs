using System;
using System.Collections.Generic;
using System.IO;
using Kros.TroubleShooterServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kros.TroubleShooterServer.Controllers
{
    [Route("api/[controller]")]
    public class UpdateFilesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<SignedSource> Get()
        {
            List<SignedSource> sources = new List<SignedSource>();
            foreach (string sourceFile in Directory.GetFiles("UpdateFiles", "*.cs"))
            {
                sources.Add(new SignedSource()
                {
                    FileName = Path.GetFileName(sourceFile),
                    SourceCode = System.IO.File.ReadAllText(sourceFile),
                    Signature = new Random().Next().ToString()
                });
            }
            return sources;
        }
    }
}
