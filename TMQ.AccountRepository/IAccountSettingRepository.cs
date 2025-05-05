using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountDomains.Entities;
using TMQ.AccountReadModels;

namespace TMQ.AccountRepository
{
    public interface IAccountSettingRepository
    {
        Task<RAccountSetting?> GetById(string id);
        Task<RAccountSetting[]> GetByUserId(string userId);
        Task AddOrChange(AccountSetting accountSetting);
    }
}
