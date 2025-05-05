using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;
using TMQ.HttpClientBase;

namespace TMQ.GoogleService
{
    public class GoogleReCaptChaService(string domain, IHttpClient httpClient) : IGoogleReCaptChaService
    {
        public async Task<bool> Valid(string secretKey, string? recaptcha, string remoteIp)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                return false;
            }

            if (string.IsNullOrEmpty(recaptcha))
            {
                return false;
            }

            string url = $"{domain}?secret={secretKey}&response={recaptcha}&remoteip={remoteIp}";
            HttpResponseMessage googleResponse = await httpClient.Post(url, string.Empty);
            if (googleResponse.IsSuccessStatusCode)
            {
                string data = await googleResponse.Content.ReadAsStringAsync();
                GoogleReCaptChaResponse? obj = Serialize.JsonDeserializeObject<GoogleReCaptChaResponse>(data);
                return obj?.success == true;
            }

            return false;
        }
    }
}
