using Microsoft.AspNetCore.Authorization;
using TMQ.BaseApplication.Services;
using TMQ.BaseCommands;
using TMQ.Common;
using TMQ.EnumDefine;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemManager.Shared;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;

namespace TMQ.SystemManager.Services
{
    public class ConfigService(
     ContextService contextService,
     IConfigRepository configRepository,
     ICommonService commonService,
     ILogger<ConfigService> logger,
     ICacheService cacheService)
     : BaseService(logger, contextService), IConfigService
    {
        public async Task<BaseCommandResponse> Add(ConfigAddCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                ConfigDomain configDomain = new ConfigDomain(command);
                await configRepository.Add(configDomain);
                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse<string>> GetTYTAppMenu()
        {
            return await ProcessCommand<string>(async (response) =>
            {
                string configKey = "TYTAppMenu";
                var config = await configRepository.GetConfigByKey(configKey);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = EnumDefine.StatusEnum.Active,
                        Key = configKey,
                        Description = "TYTAppMenu Config",
                        Value = string.Empty,
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(configKey);
                }

                response.Data = config?.Value.AsString();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<string>> LanguageConfigByCompany(string companyId)
        {
            return await ProcessCommand<string>(async (response) =>
            {
                string key = $"LanguageConfigByCompany_{companyId}";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = EnumDefine.StatusEnum.Active,
                        Description = "LanguageConfigByCompany",
                        Key = key,
                        Value = "vn",
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = config.Value;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<string[]>> ApproveArticleEmailByWebsite(
            ApproveArticleEmailByWebsiteQuery query)
        {
            return await ProcessCommand<string[]>(async (response) =>
            {
                string key = $"ApproveArticleEmailByWebsite_{query.DealerId}";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    string[] severGroups = ["vansang.hoang@vietnamnet.vn"];
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "ApproveArticleEmailByWebsite",
                        Key = key,
                        Value = Serialize.JsonSerializeObject(severGroups),
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = Serialize.JsonDeserializeObject<string[]>(config!.Value);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<string[]>> AttributeUnits()
        {
            return await ProcessCommand<string[]>(async (response) =>
            {
                string key = $"AttributeUnits";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    string[] domainsAccept =
                    {
                    "kg"
                };
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "AttributeUnits",
                        Key = key,
                        Value = Serialize.JsonSerializeObject(domainsAccept),
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = Serialize.JsonDeserializeObject<string[]>(config.Value);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<string>> ProductType()
        {
            return await ProcessCommand<string>(async (response) =>
            {
                string key = $"ProductType";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "ProductType",
                        Key = key,
                        Value = "1",
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = config.Value;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<int>> OtpLimitConfig(ROtpLimitConfigQuery query)
        {
            return await ProcessCommand<int>(async (response) =>
            {
                string key = "OtpLimitConfig";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = EnumDefine.StatusEnum.Active,
                        Description = "OtpLimitConfig",
                        Key = key,
                        Value = "5",
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = config!.Value.AsInt();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<int>> IpLimitConfig(RIpLimitConfigQuery query)
        {
            return await ProcessCommand<int>(async (response) =>
            {
                string key = "OtpIpLimitConfig";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = EnumDefine.StatusEnum.Active,
                        Description = "IpLimitConfig",
                        Key = key,
                        Value = "100",
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = config!.Value.AsInt();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RConfig>> EmailSendMarketingConfig()
        {
            return await ProcessCommand<RConfig>(async (response) =>
            {
                string key = "EmailSendMarketingConfig.v1";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "VietNamNet Podcast",
                        Key = key,
                        Value = "podcast@vietnamnet.vn",
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = config;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<string[]?>> PromotionCategoryConfig()
        {
            return await ProcessCommand<string[]?>(async (response) =>
            {
                var key = "PromotionCategoryConfig";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "Cấu hình khuyến mãi category",
                        Key = key,
                        Value = "", // [{id: "01X1H"}]
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = Serialize.JsonDeserializeObject<string[]?>(config?.Value);
                ;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RNewsInstructionCategoryConfig[]?>> InstructionCategoryConfig()
        {
            return await ProcessCommand<RNewsInstructionCategoryConfig[]?>(async (response) =>
            {
                var key = "InstructionCategoryConfig";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "Cấu hình những category hiển thị trên app, màn hình THÔNG TIN HƯỚNG DẪN",
                        Key = key,
                        Value = "", // [{id: "01X1H", order: 1}]
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = Serialize.JsonDeserializeObject<RNewsInstructionCategoryConfig[]?>(config?.Value);
                ;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<string[]?>> CustomerAppGetVersions()
        {
            return await ProcessCommand<string[]?>(async (response) =>
            {
                string key = $"CustomerAppGetVersions";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    List<string> versionDefault = ["0.1", "0.2", "1"];
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "CustomerAppGetVersions",
                        Key = key,
                        Value = Serialize.JsonSerializeObject(versionDefault),
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = Serialize.JsonDeserializeObject<string[]>(config!.Value);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<string?>> CustomerAppGetLogo()
        {
            return await ProcessCommand<string?>(async (response) =>
            {
                string key = $"CustomerAppGetLogo";
                var config = await configRepository.GetConfigByKey(key);
                if (config == null)
                {
                    await Add(new ConfigAddCommand()
                    {
                        Status = StatusEnum.Active,
                        Description = "CustomerAppGetLogo",
                        Key = key,
                        Value = string.Empty,
                        LoginUid = string.Empty,
                        ProcessUid = string.Empty,
                        ObjectId = string.Empty
                    });
                    config = await configRepository.GetConfigByKey(key);
                }

                response.Data = config!.Value;
                response.SetSuccess();
            });
        }
    }
}
