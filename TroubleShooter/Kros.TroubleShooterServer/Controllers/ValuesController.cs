using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kros.TroubleShooterServer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Kros.TroubleShooterServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<SignedSource> Get()
        {
            return new SignedSource[] {
                new SignedSource(){ SourceCode = "source1", Signature = "sig1"},
                new SignedSource(){ SourceCode = "source2", Signature = "sig2"}
            };
        }
    }
}
