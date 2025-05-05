using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Xml;
using TMQ.Common;

namespace TMQ.HttpClientBase
{
    public class HttpClient(System.Net.Http.HttpClient client) : IHttpClient
    {
        public async Task<HttpResponseMessage> Get(string uri, string? authorizationToken = null,
            string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Get(Uri uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> GetWithUserAgent(string uri, string? userAgent, bool isAuthorization,
            string? hostName)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            if (userAgent?.Length > 0)
            {
                requestMessage.Headers.Add("User-Agent", userAgent);
            }

            if (isAuthorization)
            {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue("local", "1");
            }

            if (hostName?.Length > 0)
            {
                requestMessage.Headers.Host = hostName;
            }

            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> HeadWithUserAgent(string uri, string? userAgent)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Head, uri);
            if (userAgent?.Length > 0)
            {
                requestMessage.Headers.Add("User-Agent", userAgent);
            }

            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Get(string uri, IDictionary<string, string>? headers)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            if (headers?.Count > 0)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Post<T>(string uri, T item, string? authorizationToken = null,
            string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            string json = Serialize.JsonSerializeObject(item);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = string.IsNullOrEmpty(authorizationMethod)
                    ? new AuthenticationHeaderValue(authorizationToken)
                    : new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string uri, MultipartFormDataContent content)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = content
            };
            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string uri, FormUrlEncodedContent content)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = content
            };
            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string uri, string? content)
        {
            var requestMessage = content?.Length > 0
                ? new HttpRequestMessage(HttpMethod.Post, uri)
                {
                    Content = new StringContent(content, Encoding.UTF8, "application/json")
                }
                : new HttpRequestMessage(HttpMethod.Post, uri);
            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Post(string uri, IDictionary<string, string>? headers)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            if (headers?.Count > 0)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.Add(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Post<T>(string uri, T item, IDictionary<string, string>? headers)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri);
            string json = Serialize.JsonSerializeObject(item);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            if (headers?.Count > 0)
            {
                foreach (var header in headers)
                {
                    requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            var response = await client.SendAsync(requestMessage);
            return response;
        }

        public async Task<HttpResponseMessage> Put<T>(string uri, T item, string? authorizationToken = null,
            string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            string json = Serialize.JsonSerializeObject(item);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = string.IsNullOrEmpty(authorizationMethod)
                    ? new AuthenticationHeaderValue(authorizationToken)
                    : new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            return response;
        }

        public async Task<HttpResponseMessage> Put(string uri, string item, string? authorizationToken = null,
            string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, uri);
            requestMessage.Content = new StringContent(item, Encoding.UTF8, "application/json");

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization = string.IsNullOrEmpty(authorizationMethod)
                    ? new AuthenticationHeaderValue(authorizationToken)
                    : new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            return response;
        }

        public async Task<HttpResponseMessage> Delete(string uri, string? authorizationToken = null,
            string authorizationMethod = "Bearer")
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            if (authorizationToken != null)
            {
                requestMessage.Headers.Authorization =
                    new AuthenticationHeaderValue(authorizationMethod, authorizationToken);
            }

            return await client.SendAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> Post(string uri, StringContent content)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = content
            };
            return await client.SendAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> Post(string uri, string action, XmlDocument doc)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(doc.InnerXml, Encoding.UTF8, "text/xml")
            };
            requestMessage.Headers.Add("SOAPAction", action);

            return await client.SendAsync(requestMessage);
        }

        public async Task<HttpResponseMessage> PostSoap(string uri, string action, string content)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(content, Encoding.UTF8, "text/xml")
            };
            requestMessage.Headers.Add("SOAPAction", action);

            return await client.SendAsync(requestMessage);
        }

        public async Task<string> Upload(string url, string fileName, byte[] bytes)
        {
            try
            {
                var fileContent = new ByteArrayContent(bytes);
                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                {
                    FileName = fileName,
                    Name = fileName,
                };
                using var content = new MultipartFormDataContent();
                content.Add(fileContent, "files");
                var response = await client.PostAsync(url, content);
                if (response.IsSuccessStatusCode)
                {
                    var p = await response.Content.ReadAsStringAsync();
                    return p;
                }

                throw new Exception($"{(int)response.StatusCode} ({response.ReasonPhrase})");
            }
            catch (HttpRequestException e)
            {
                e.Data["Param"] = new { fileName, bytes };
                throw;
            }
            catch (Exception e)
            {
                e.Data["Param"] = new { fileName, bytes };
                throw;
            }
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption,
            CancellationToken cancellationToken)
        {
            return client.SendAsync(request, completionOption, cancellationToken);
        }
    }
}
