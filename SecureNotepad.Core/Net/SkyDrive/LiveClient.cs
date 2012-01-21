using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using SecureNotepad.Core.Net.OAuth;

namespace SecureNotepad.Core.Net.SkyDrive
{
    public class LiveClient : HttpClient
    {
        private const string BASE_URL = "https://apis.live.net";
        private const string API_VERSION = "v5.0";

        public string AccessToken { get; set; }

        public LiveClient()
        {
            BaseAddress = new Uri(BASE_URL);
        }

        public LiveClient(string accessToken) : this()
        {
            AccessToken = accessToken;
        }

        public void Execute(Action<Task<String>> resultCallback, string path, string httpMethod, string contentType = null)
        {
            path = BuildPath(path);
            Task<String> t;

            switch (httpMethod)
            {
                default:
                    t = GetStringAsync(path);
                    break;
            }

            t.ContinueWith(resultCallback);            
        }

        private string BuildPath(string path)
        {
            return API_VERSION + path + "?access_token=" + AccessToken;
        }

    }
}
