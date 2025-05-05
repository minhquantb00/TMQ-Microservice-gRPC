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
    public interface IConfigService
    {
        [OperationContract]
        Task<BaseCommandResponse> Add(ConfigAddCommand command);

        [OperationContract]
        Task<BaseCommandResponse<string>> GetTYTAppMenu();

        [OperationContract]
        Task<BaseCommandResponse<string>> LanguageConfigByCompany(string companyId);

        [OperationContract]
        Task<BaseCommandResponse<string[]>> ApproveArticleEmailByWebsite(
            ApproveArticleEmailByWebsiteQuery query);

        [OperationContract]
        Task<BaseCommandResponse<string[]>> AttributeUnits();

        [OperationContract]
        Task<BaseCommandResponse<string>> ProductType();

        [OperationContract]
        Task<BaseCommandResponse<int>> OtpLimitConfig(ROtpLimitConfigQuery query);
        [OperationContract]
        Task<BaseCommandResponse<int>> IpLimitConfig(RIpLimitConfigQuery query);

        [OperationContract]
        Task<BaseCommandResponse<RConfig>> EmailSendMarketingConfig();

        [OperationContract]
        Task<BaseCommandResponse<RNewsInstructionCategoryConfig[]?>> InstructionCategoryConfig();
        [OperationContract]
        Task<BaseCommandResponse<string[]?>> CustomerAppGetVersions();
        [OperationContract]
        Task<BaseCommandResponse<string?>> CustomerAppGetLogo();

    }
}
