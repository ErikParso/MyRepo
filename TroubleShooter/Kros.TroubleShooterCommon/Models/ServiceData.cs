using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Kros.TroubleShooterCommon.Models
{
    public class ServiceData
    {
        public List<ServiceParam> ServiceParams { get; set; }

        public List<IFormFile> Attachments { get; set; }

        public ServiceData()
        {
            ServiceParams = new List<ServiceParam>();
            Attachments = new List<IFormFile>();
        }
     }

    public class ServiceParam
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }
}
