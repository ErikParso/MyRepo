using System;
using System.IO;
using System.Net.Http;

namespace Kros.TroubleShooterClient
{
    /// <summary>
    /// HttpClient to communicate with web api 
    /// </summary>
    public class TroubleShooterClient : HttpClient
    {
        /// <summary>
        /// controllers path
        /// </summary>
        public const string SERVICE_PATH = "api/troubleshooter";

        /// <summary>
        /// the service uri
        /// </summary>
        public const string URI = "http://olymptroubleshooter.azurewebsites.net";
        //public const string URI = "http://localhost:10003";

        /// <summary>
        /// init this client
        /// </summary>
        public TroubleShooterClient()
            : base()
        {
            BaseAddress = new Uri(URI);
        }

        /// <summary>
        /// Tries to communicate with server
        /// </summary>
        /// <returns></returns>
        public bool TryConnection()
        {
            string uri = Path.Combine(SERVICE_PATH,"test");
            try
            {
                HttpResponseMessage response = GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
