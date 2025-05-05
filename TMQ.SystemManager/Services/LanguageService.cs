using Microsoft.AspNetCore.Authorization;
using TMQ.BaseApplication.Services;
using TMQ.BaseCommands;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemManager.Shared;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;

namespace TMQ.SystemManager.Services
{
    public class LanguageService(
        ContextService contextService,
        ILanguageRepository languageRepository,
        ILogger<LanguageService> logger)
        : BaseService(logger, contextService), ILanguageService
    {
        private readonly ILanguageRepository _languageRepository = languageRepository;

        [AllowAnonymous]
        public async Task<BaseCommandResponse<RLanguage[]>> Gets(LanguageGetsQuery query)
        {
            return await ProcessCommand<RLanguage[]>(async (response) =>
            {
                RLanguage[] langs = await _languageRepository.Gets();
                response.Data = langs;
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Add(LanguageAddCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                Language language = new Language(command);
                await _languageRepository.Add(language);
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> Update(LanguageChangeCommand command)
        {
            return await ProcessCommand(async (response) =>
            {
                if (string.IsNullOrEmpty(command.Id))
                {
                    response.SetFail($"Language input can't null {command.Id}!");
                    return;
                }

                RLanguage? rLanguage = await _languageRepository.GetById(command.Id);
                if (rLanguage == null)
                {
                    response.SetFail("Language not exist");
                    return;
                }

                Language language = new Language(rLanguage);
                language.Change(command);
                await _languageRepository.Update(language);
                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse<RLanguage[]>> GetByType(LanguageGetByTypeQuery query)
        {
            return await ProcessCommand<RLanguage[]>(async (response) =>
            {
                RLanguage[] langs = await _languageRepository.GetByType(query.Type);
                response.Data = langs;
                response.SetSuccess();
            });
        }
    }
}
