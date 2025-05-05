using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseCommands;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Queries;
using TMQ.SystemReadModels;

namespace TMQ.SystemManager.Shared
{
    [ServiceContract]
    public interface ILocaleStringResourceService
    {
        [OperationContract]
        Task<BaseCommandResponse<RLocaleStringResource[]?>> Gets(LocaleSearchCommand locale);

        [OperationContract]
        Task<BaseCommandResponse> Insert(LocaleInsertCommand locale);

        [OperationContract]
        Task<BaseCommandResponse> Inserts(LocaleInsertCommand[] commands);

        [OperationContract]
        Task<BaseCommandResponse> AddIfNotExist(LocaleInsertCommand[] commands);

        [OperationContract]
        Task<BaseCommandResponse> Update(LocaleUpdateCommand locale);

        [OperationContract]
        Task<BaseCommandResponse> Delete(LocaleDeleteCommand locale);

        [OperationContract]
        Task<BaseCommandResponse<RLocaleStringResource[]>> GetByLanguageId(
            LocaleStringResourceGetByLanguageIdQuery query);

        [OperationContract]
        Task<BaseCommandResponse<string[]>> CheckInValidLocaleInsert(LocaleInsertCommand[] locales);

        [OperationContract]
        Task<BaseCommandResponse<RLocaleStringResource[]>> GetByKeys(LocaleStringResourceGetByKeysQuery query);
    }
}
