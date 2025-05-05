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
    public class GoogleAccountService(IHttpClient httpClient, string url) : IGoogleAccountService
    {
        public async Task<TokenInfoResponse?> Get(string accountToken)
        {
            string requestUrl = $"{url}?id_token={accountToken}";
            HttpResponseMessage googleResponse = await httpClient.Get(requestUrl);
            if (googleResponse.IsSuccessStatusCode)
            {
                string data = await googleResponse.Content.ReadAsStringAsync();
                TokenInfoResponse? obj = Serialize.JsonDeserializeObject<TokenInfoResponse>(data);
                return obj;
            }
            return null;
        }
    }
}
