using TMQ.BaseApplication.Models;
using TMQ.Common;
using TMQ.HttpClientBase;

namespace TMQ.BaseApplication.Services
{
    public class ZaloAccountService(IHttpClient httpClient, string url)
    {
        public async Task<ZaloInfoResponse?> Get(string accountToken)
        {
            using var graphHttpRequest = new HttpRequestMessage(HttpMethod.Get, url);
            graphHttpRequest.Headers.Add("access_token", accountToken);

            HttpResponseMessage graphHttpResponse = await httpClient.SendAsync(graphHttpRequest, HttpCompletionOption.ResponseContentRead, CancellationToken.None);
            graphHttpResponse.EnsureSuccessStatusCode();

            if (graphHttpResponse.IsSuccessStatusCode)
            {
                ZaloInfoResponse? obj = Serialize.JsonDeserializeObject<ZaloInfoResponse>(await graphHttpResponse.Content.ReadAsStringAsync());
                return obj;
            }
            return null;
        }
    }
}
