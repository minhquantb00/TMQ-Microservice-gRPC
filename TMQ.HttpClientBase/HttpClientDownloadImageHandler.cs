using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.HttpClientBase
{
    public class HttpClientDownloadImageHandler : HttpClientHandler
    {
        public HttpClientDownloadImageHandler()
        {
            AutomaticDecompression = DecompressionMethods.All;
            MaxConnectionsPerServer = 1000;
            AllowAutoRedirect = true;
            MaxAutomaticRedirections = 2;
        }
    }
}
