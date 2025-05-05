using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TMQ.Common;
using TMQ.HttpClientBase;

namespace TMQ.GoogleService
{
    public interface IGoogleAuthenticationService
    {
        string GetLoginUrl(string sessionKey, out string state);
        Task<GoogleAuthenticationGetTokenResponse?> GetToken(string state, string code, string sessionKey);
        Task<TokenInfoResponse?> GetUserInfo(string accountToken);
        Task<GoogleUserInfoV2?> GetUserInfoV2(string accountToken);
    }

    public class GoogleAuthenticationService(
        IHttpClient httpClient,
        string clientId,
        string clientSecret,
        string loginUrl,
        string returnUrl,
        string tokenUrl,
        string getUserInfoUrl)
        : IGoogleAuthenticationService
    {
        private const string EncryptionKey = "bjW4H#;Y+$N'z3vUD*e:(}eZLIIyov>4";

        public string GetLoginUrl(string sessionKey, out string state)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(loginUrl);
            builder.Append("?response_type=code");
            builder.Append($"&client_id={clientId}");
            builder.Append($"&redirect_uri={WebUtility.UrlEncode(returnUrl)}");
            builder.Append("&scope=openid%20profile%20email");
            builder.Append("&access_type=offline");
            state =
                EncryptionExtensions.Md5($"{sessionKey}{returnUrl}{EncryptionKey}{clientId}{clientSecret}");
            builder.Append($"&state={state}");
            return builder.ToString();
        }

        public async Task<GoogleAuthenticationGetTokenResponse?> GetToken(string state, string code, string sessionKey)
        {
            string stateCompare =
                EncryptionExtensions.Md5($"{sessionKey}{returnUrl}{EncryptionKey}{clientId}{clientSecret}");
            if (state != stateCompare)
            {
                throw new Exception("state invalid");
            }

            StringBuilder getTokenUrl = new StringBuilder();
            getTokenUrl.Append(tokenUrl);
            getTokenUrl.Append($"?code={code}");
            getTokenUrl.Append($"&client_id={clientId}");
            getTokenUrl.Append($"&client_secret={clientSecret}");
            getTokenUrl.Append($"&redirect_uri={returnUrl}");
            getTokenUrl.Append("&scope=openid%20profile%20email");
            getTokenUrl.Append("&access_type=offline");
            getTokenUrl.Append("&grant_type=authorization_code");
            string url = getTokenUrl.ToString();
            var response = await httpClient.Post(url, new { });
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                GoogleAuthenticationGetTokenResponse? model =
                    Serialize.JsonDeserializeObject<GoogleAuthenticationGetTokenResponse>(content);
                return model;
            }
            throw new Exception($"Get Token fail:{response.StatusCode}---{response.ReasonPhrase}");
        }

        public async Task<TokenInfoResponse?> GetUserInfo(string accountToken)
        {
            string requestUrl = $"{getUserInfoUrl}";
            HttpResponseMessage googleResponse = await httpClient.Post(requestUrl, new { }, accountToken);
            if (googleResponse.IsSuccessStatusCode)
            {
                string data = await googleResponse.Content.ReadAsStringAsync();
                TokenInfoResponse? obj = Serialize.JsonDeserializeObject<TokenInfoResponse>(data);
                return obj;
            }
            return null;
        }

        public async Task<GoogleUserInfoV2?> GetUserInfoV2(string accountToken)
        {
            string url = "https://people.googleapis.com/v1/people/me?personFields=birthdays,genders";
            HttpResponseMessage googleResponse = await httpClient.Get(url, accountToken);
            if (googleResponse.IsSuccessStatusCode)
            {
                string data = await googleResponse.Content.ReadAsStringAsync();
                GoogleUserInfoV2? obj = Serialize.JsonDeserializeObject<GoogleUserInfoV2>(data);
                return obj;
            }
            return null;
        }
    }

    public class GoogleAuthenticationGetTokenResponse
    {
        public string? access_token { get; set; }
        public string? expires_in { get; set; }
        public string? token_type { get; set; }
        public string? scope { get; set; }
        public string? refresh_token { get; set; }
        public string? error { get; set; }
        public string? error_description { get; set; }
    }
}
