using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountDomains.Entities;
using TMQ.AccountReadModels;
using TMQ.BaseRepositories;

namespace AccountRepositorySQLImplement
{
    public interface IUserRepository : ISqlDbBaseRepository<User>
    {
        Task<RUser?> Get(string id);
        Task<RUser[]> Gets(string[] ids);
        Task<RUser?> GetByUserNameOrEmailOrPhoneNumber(string keyword);
        Task<RUser?> GetByUserName(string userName);
        Task Add(IDbConnection dbConnection, IDbTransaction dbTransaction, User user);
        Task PasswordChange(User user);
        Task TwoFactorChange(User user);
        Task PhoneNumberChange(User user);
    }
}
