using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Kros.TroubleShooterClient.Update
{
    public class Updater
    {
        private string _updateDir;
        private const string URI_GET_VERSION = "api/values"; 

        private HttpClient client = new HttpClient();

        public Updater(string updateDir)
        {
            _updateDir = updateDir;
            client.BaseAddress = new Uri("http://localhost:51131/");
            //client.DefaultRequestHeaders.Accept.Clear();
            //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        } 

        public SignedSource[] Execute()
        {
            SignedSource[] sourceFile = null;
            HttpResponseMessage response = client.GetAsync(URI_GET_VERSION).GetAwaiter().GetResult();
            if(response.IsSuccessStatusCode)
                sourceFile = response.Content.ReadAsAsync<SignedSource[]>().GetAwaiter().GetResult();
            return sourceFile;
        }


    }
}
