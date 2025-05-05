using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountDomains.Entities;
using TMQ.AccountReadModels;
using TMQ.EnumDefine;

namespace TMQ.AccountRepository
{
    public interface IExternalLoginRepository
    {
        Task<RExternalLogin?> GetById(string id);
        Task<RExternalLogin[]> Get(string userId);
        Task Change(ExternalLogin externalLogin);
        Task StatusChange(ExternalLogin externalLogin);
        Task Add(ExternalLogin externalLogin, User? user, Func<IDbConnection, IDbTransaction, User, Task>? add);
        Task<RExternalLogin?> GetByProviderKey(string key, ExternalLoginProviderEnum loginProvider);
    }
}
