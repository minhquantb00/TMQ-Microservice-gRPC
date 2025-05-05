using System.Net;
using System.Text;
using TMQ.BaseApplication.Models;
using TMQ.Common;
using TMQ.HttpClientBase;

namespace TMQ.BaseApplication.Services
{
    public class FacebookAuthenticationService(
    string clientId,
    string clientSecret,
    string loginUrl,
    string returnUrl,
    string tokenUrl,
    string getUserInfoUrl,
    IHttpClient httpClient)
    {
        private const string EncryptionKey = "bjW4H#;Y+$N'z3vUD*e:(}eZLIIyov>4";

        public string GetLoginUrl(string sessionKey, out string state)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(loginUrl);
            builder.Append($"?client_id={clientId}");
            builder.Append($"&scope=email");
            builder.Append($"&response_type=code");
            builder.Append("&redirect_uri=" + WebUtility.UrlEncode(returnUrl));
            builder.Append("&access_type=offline");
            state = EncryptionExtensions.Md5(sessionKey + returnUrl + EncryptionKey + clientId + clientSecret);
            builder.Append($"&state={state}");
            return builder.ToString();
        }

        public async Task<FacebookAuthenticationGetTokenResponse?> GetToken(string state, string code, string sessionKey)
        {
            string stateCompare =
                EncryptionExtensions.Md5(sessionKey + returnUrl + EncryptionKey + clientId + clientSecret);
            if (state != stateCompare)
            {
                throw new Exception("state invalid");
            }

            StringBuilder getTokenUrl = new StringBuilder();
            getTokenUrl.Append(tokenUrl);
            getTokenUrl.Append($"?client_id={clientId}");
            getTokenUrl.Append($"&redirect_uri={returnUrl}");
            getTokenUrl.Append($"&client_secret={clientSecret}");
            getTokenUrl.Append($"&code={code}");
            string url = getTokenUrl.ToString();
            var response = await httpClient.Get(url);
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                FacebookAuthenticationGetTokenResponse? model =
                    Serialize.JsonDeserializeObject<FacebookAuthenticationGetTokenResponse>(content);
                return model;
            }

            throw new Exception($"Get Token fail:{response.StatusCode}---{response.ReasonPhrase}");
        }

        public async Task<FacebookAuthenticationAccountInfoResponse?> GetUserInfo(string accountToken)
        {
            string requestUrl =
                $"{getUserInfoUrl}?fields=id,first_name,last_name,middle_name,name,name_format,picture,short_name,email&access_token={accountToken}";
            HttpResponseMessage googleResponse = await httpClient.Get(requestUrl);
            if (googleResponse.IsSuccessStatusCode)
            {
                string data = await googleResponse.Content.ReadAsStringAsync();
                FacebookAuthenticationAccountInfoResponse? obj =
                    Serialize.JsonDeserializeObject<FacebookAuthenticationAccountInfoResponse>(data);
                return obj;
            }

            return null;
        }
    }
}
