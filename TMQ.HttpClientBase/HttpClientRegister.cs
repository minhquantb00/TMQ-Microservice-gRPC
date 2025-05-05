using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Polly.Timeout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.HttpClientBase
{
    public static class HttpClientRegister
    {
        public static IServiceCollection RegisterHttpClient(this IServiceCollection services)
        {
            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .Or<TimeoutRejectedException>() // thrown by Polly's TimeoutPolicy if the inner call times out
                .WaitAndRetryAsync([
                    TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(20),
                TimeSpan.FromSeconds(30)
                ]);

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(90); // Timeout for an individual try
            services.AddTransient<TMQHttpClientHandler>();
            services.AddTransient<HttpClientDownloadImageHandler>();
            services.AddTransient<HttpClientServerCertificateCustomValidationHandler>();
            services.AddHttpClient(HttpClientNameEnum.Default.ToString(), client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
                client.Timeout = TimeSpan.FromSeconds(60);
            }).ConfigurePrimaryHttpMessageHandler<TMQHttpClientHandler>();

            services.AddHttpClient(HttpClientNameEnum.Es.ToString(), client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
                client.Timeout = TimeSpan.FromSeconds(600);
            }).ConfigurePrimaryHttpMessageHandler<HttpClientHandler>();

            services.AddHttpClient(HttpClientNameEnum.Upload.ToString(), client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
                client.Timeout = TimeSpan.FromSeconds(600);
            }).ConfigurePrimaryHttpMessageHandler<HttpClientHandler>();

            services.AddHttpClient(HttpClientNameEnum.Retry.ToString(), client =>
            {
                client.DefaultRequestHeaders.Add("User-Agent",
                    "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/59.0.3071.115 Safari/537.36");
                client.Timeout = TimeSpan.FromSeconds(60);
            })
                .AddPolicyHandler(retryPolicy)
                .AddPolicyHandler(timeoutPolicy)
                .ConfigurePrimaryHttpMessageHandler<HttpClientHandler>();
            services.AddTransient<HttpClientResolver>(serviceProvider => key =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(key);
                return new HttpClient(httpClient);
            });
            services.AddTransient<HttpClientResolverWithToken>(serviceProvider => (key, token) =>
            {
                var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient(key);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return new HttpClient(httpClient);
            });
            return services;
        }
    }
}
