using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Kros.TroubleShooterClient.Update
{
    public class Updater
    {
        private string _updateDir;
        private const string URI_GET_VERSION = "api/updateFiles"; 

        private HttpClient client = new HttpClient();

        public Updater(string updateDir)
        {
            _updateDir = updateDir;
            client.BaseAddress = new Uri("http://localhost:51131/");
        }

        public void Execute()
        {
            IEnumerable<ProtectedSource> sources = new List<ProtectedSource>();
            HttpResponseMessage response = client.GetAsync(URI_GET_VERSION).GetAwaiter().GetResult();
            if (response.IsSuccessStatusCode)
                sources = response.Content.ReadAsAsync<IEnumerable<ProtectedSource>>().GetAwaiter().GetResult();
            if (!Directory.Exists(_updateDir))
                Directory.CreateDirectory(_updateDir);
            foreach (ProtectedSource source in sources)
                File.WriteAllText(Path.Combine(_updateDir, source.FileName), source.SourceCode);            
        }
    }
}
