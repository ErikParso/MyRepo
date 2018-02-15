using Kros.TroubleShooterClient.ViewModel;
using Kros.TroubleShooterCommon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Kros.TroubleShooterClient.Service
{
    public class ServiceManager
    {
        private HttpClient client = new HttpClient();
        private const string URI_GET_VERSION = "api/updateFiles";

        public ServiceManager()
        {
            client.BaseAddress = new Uri("http://localhost:51131/");
        }

        public bool SendToServis(IEnumerable<string> attachments, IEnumerable<OptionalServiceProp> properties)
        {
            string uri = (URI_GET_VERSION + "/service");
            MultipartFormDataContent form = new MultipartFormDataContent();
            foreach (string attachment in attachments)
            {
                using (FileStream fs = new FileStream(attachment, FileMode.OpenOrCreate))
                {
                    byte[] data;
                    using (var br = new BinaryReader(fs))
                        data = br.ReadBytes((int)fs.Length);
                    ByteArrayContent bytes = new ByteArrayContent(data);
                    form.Add(bytes, "file", Path.GetFileName(attachment));
                }
            }

            foreach (OptionalServiceProp prop in properties)
            {
                form.Add(new StringContent(prop.Value?? ""), prop.Name);
            }

            HttpResponseMessage response = client.PostAsync(uri, form).GetAwaiter().GetResult();
            return true;
        }

    }
}
