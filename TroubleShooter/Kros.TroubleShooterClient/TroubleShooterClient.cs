using System;
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
        public const string SERVICE_PATH = "api/updateFiles";

        /// <summary>
        /// init this client
        /// </summary>
        public TroubleShooterClient()
            : base()
        {
            BaseAddress = new Uri("http://localhost:51131/");
        }

        /// <summary>
        /// Tries to communicate with server
        /// </summary>
        /// <returns></returns>
        public bool TryConnection()
        {
            string uri = (SERVICE_PATH + "/test");
            try
            {
                HttpResponseMessage response = GetAsync(uri).GetAwaiter().GetResult();
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch (HttpRequestException e)
            {
                return false;
            }

        }
    }
}
