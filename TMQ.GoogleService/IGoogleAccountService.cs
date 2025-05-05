using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.GoogleService
{
    public interface IGoogleAccountService
    {
        Task<TokenInfoResponse?> Get(string accountToken);
    }
}
