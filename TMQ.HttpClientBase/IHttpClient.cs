using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace TMQ.HttpClientBase
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> Get(string uri, string? authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> Get(Uri uri);
        Task<HttpResponseMessage> GetWithUserAgent(string uri, string? userAgent, bool isAuthorization, string? hostName);
        Task<HttpResponseMessage> HeadWithUserAgent(string uri, string? userAgent);

        Task<HttpResponseMessage> Get(string uri, IDictionary<string, string>? headers);

        Task<HttpResponseMessage> Post<T>(string uri, T item, string? authorizationToken = null,
            string authorizationMethod = "Bearer");


        Task<HttpResponseMessage> Post(string uri, MultipartFormDataContent content);
        Task<HttpResponseMessage> Post(string uri, FormUrlEncodedContent content);

        Task<HttpResponseMessage> Post(string uri, string? content);

        Task<HttpResponseMessage> Post(string uri);
        Task<HttpResponseMessage> Post(string uri, IDictionary<string, string>? headers);
        Task<HttpResponseMessage> Post<T>(string uri, T item, IDictionary<string, string>? headers);

        Task<HttpResponseMessage> Delete(string uri, string? authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> Put<T>(string uri, T item, string? authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> Put(string uri, string item, string? authorizationToken = null,
            string authorizationMethod = "Bearer");

        Task<HttpResponseMessage> Post(string uri, StringContent content);

        Task<HttpResponseMessage> Post(string uri, string action, XmlDocument doc);

        Task<HttpResponseMessage> PostSoap(string uri, string action, string content);

        Task<string> Upload(string url, string fileName, byte[] bytes);

        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken);
    }
}
