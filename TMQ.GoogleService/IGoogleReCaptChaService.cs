using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.GoogleService
{
    public interface IGoogleReCaptChaService
    {
        Task<bool> Valid(string secretKey, string response, string remoteIp);
    }
}
