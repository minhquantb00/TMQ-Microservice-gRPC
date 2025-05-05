using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.HttpClientBase
{
    public class HttpClientServerCertificateCustomValidationHandler : HttpClientHandler
    {
        public HttpClientServerCertificateCustomValidationHandler()
        {
            AutomaticDecompression = DecompressionMethods.All;
            MaxConnectionsPerServer = 1000;
            AllowAutoRedirect = false;
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
        }
    }
}
