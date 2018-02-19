using Kros.TroubleShooterClient.Model;
using Kros.TroubleShooterClient.ViewModel;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Kros.TroubleShooterClient.Service
{
    /// <summary>
    /// Sends information to servis if server is running
    /// </summary>
    public class ServisManager
    {
        /// <summary>
        /// uses MultipartFormDataContent to send 
        /// attachments and servis data to server
        /// </summary>
        /// <param name="attachments">attachments - files</param>
        /// <param name="properties"><see cref="OptionalServiceProp"/> data sent to server</param>
        /// <returns>if data was succesfully send to server</returns>
        public bool SendToServis(IEnumerable<string> attachments, IEnumerable<OptionalServiceProp> properties)
        {
            string uri = (TroubleShooterClient.SERVICE_PATH + "/service");
            //create httpContent
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
                form.Add(new StringContent(prop.Value ?? ""), prop.Name);
            }

            //post data in created HttpContent
            HttpResponseMessage response = TroubleShooter.Current.Client.PostAsync(uri, form).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
                return true;
            else return false;
        }
    }
}
