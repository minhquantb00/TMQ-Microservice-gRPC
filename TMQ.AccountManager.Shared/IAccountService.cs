using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TMQ.AccountCommands.Commands;
using TMQ.AccountCommands.Events;
using TMQ.AccountDomains.Entities;
using TMQ.AccountReadModels;
using TMQ.BaseCommands;

namespace TMQ.AccountManager.Shared
{
    [ServiceContract]
    public interface IAccountService
    {
        //[OperationContract]
        //Task<BaseCommandResponse<User?>> GetByKey(AccountGetByKeyQuery query);
        //[OperationContract]
        //Task<BaseCommandResponse<RUser[]?>> Gets(AccountGetsQuery query);
        //[OperationContract]
        //Task<BaseCommandResponse<RUser>> GetById(AccountGetByIdQuery query);
        //[OperationContract]
        //Task<BaseCommandResponse<RUser[]>> GetByIds(AccountGetByIdsQuery query);
        [OperationContract]
        Task<BaseCommandResponse<string>> Add(AccountAddCommand command);
        [OperationContract]
        Task<BaseCommandResponse> Change(AccountChangeCommand command);
        //[OperationContract]
        //Task<BaseCommandResponse> SetPassword(SetPasswordCommand command);
        //[OperationContract]
        //Task<BaseCommandResponse> EsSync(AccountSyncCommand command);
        Task Process(UserAddEvent @event);
        Task Process(UserChangeEvent @event);
    }
}
