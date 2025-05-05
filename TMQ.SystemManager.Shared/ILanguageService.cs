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
    public interface ILanguageService
    {
        [OperationContract]
        Task<BaseCommandResponse<RLanguage[]>> Gets(LanguageGetsQuery query);

        [OperationContract]
        Task<BaseCommandResponse> Add(LanguageAddCommand command);

        [OperationContract]
        Task<BaseCommandResponse<RLanguage[]>> GetByType(LanguageGetByTypeQuery query);

        [OperationContract]
        Task<BaseCommandResponse> Update(LanguageChangeCommand command);
    }
}
