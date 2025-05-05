using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountDomains.Entities;
using TMQ.AccountReadModels;
using TMQ.BaseReadModels;
using TMQ.EnumDefine;

namespace TMQ.AccountRepository
{
    public interface IUserDeviceMappingRepository
    {
        Task Add(UserDeviceMapping userDeviceMapping, UserDevice userDevice);
        Task Add(UserDevice userDevice, UserDevice[] userDevicesRemove);

        Task RefreshToken(UserDeviceMapping userDeviceMapping, UserDeviceMapping newUserDeviceMapping,
            UserDevice userDevice);

        Task LogOut(UserDeviceMapping userDeviceMapping, UserDevice? userDevice);
        Task<RUserDeviceMapping?> GetById(string id);
        Task<RUserDeviceMapping[]> GetByIds(string[] ids);
        Task UserDeviceTrusted(UserDevice userDevice);
        Task<RUserDevice?> UserDeviceGetById(string id);
        Task<RUserDevice[]> UserDeviceGetByIds(string[] ids);
        Task<RUserDevice[]> UserDeviceGetByLoginType(LoginTypeEnum loginType);
        Task<RUserDevice[]> UserDeviceGetByUserId(string userId);
        Task<RUserDevice[]> UserDeviceGetByUserIds(string[] userIds);
        Task AddOrUpdateUserDeviceLoginNotify(UserDeviceLoginNotify userDevice);
        Task<RUserDeviceLoginNotify?> UserDeviceLoginNotifyGetById(string id);
        Task<RUserDevice[]> UserDeviceGetByFCMToken(string fCMToken);
        Task UserDeviceChange(UserDevice userDevice);
        Task<RUserDevice[]?> UserDeviceGetByUserId(string userId, int loginType, string clientId, RefSqlPaging paging);
    }
}
