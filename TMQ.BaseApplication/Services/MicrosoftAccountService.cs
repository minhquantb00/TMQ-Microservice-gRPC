using System.Net.Http.Headers;
using TMQ.BaseApplication.Models;
using TMQ.Common;
using TMQ.HttpClientBase;

namespace TMQ.BaseApplication.Services
{
    public class MicrosoftAccountService(IHttpClient httpClient, string url)
    {
        public async Task<MicrosoftTokenResponse?> Get(string accountToken)
        {
            using var graphHttpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            graphHttpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accountToken);

            HttpResponseMessage graphHttpResponse = await httpClient.SendAsync(graphHttpRequest, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
            graphHttpResponse.EnsureSuccessStatusCode();

            if (graphHttpResponse.IsSuccessStatusCode)
            {
                MicrosoftTokenResponse? obj = Serialize.JsonDeserializeObject<MicrosoftTokenResponse>(await graphHttpResponse.Content.ReadAsStringAsync());
                return obj;
            }
            return null;
        }
    }
}
