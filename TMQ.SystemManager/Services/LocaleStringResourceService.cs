using Microsoft.AspNetCore.Authorization;
using TMQ.BaseApplication.Services;
using TMQ.BaseCommands;
using TMQ.BaseReadModels;
using TMQ.Common;
using TMQ.SystemCommands.Commands;
using TMQ.SystemCommands.Queries;
using TMQ.SystemDomains;
using TMQ.SystemManager.Shared;
using TMQ.SystemReadModels;
using TMQ.SystemRepository;

namespace TMQ.SystemManager.Services
{
    public class LocaleStringResourceService : BaseService, ILocaleStringResourceService
    {
        private readonly ILocaleStringResourceRepository _localeRepository;

        public LocaleStringResourceService(ContextService contextService,
            ILocaleStringResourceRepository localeRepository, ILogger<LocaleStringResourceService> logger) : base(
            logger, contextService)
        {
            _localeRepository = localeRepository;
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse<RLocaleStringResource[]?>> Gets(LocaleSearchCommand locale)
        {
            return await ProcessCommand<RLocaleStringResource[]?>(async (response) =>
            {
                RLocaleStringResource[]? locales =
                    await _localeRepository.Gets(locale, new RefSqlPaging(locale.PageIndex, locale.PageSize));
                response.Data = locales;
                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse> Insert(LocaleInsertCommand locale)
        {
            return await ProcessCommand(async (response) =>
            {
                var input = new Locale(locale);
                await _localeRepository.Insert(input);
                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse> Inserts(LocaleInsertCommand[] commands)
        {
            return await ProcessCommand(async (response) =>
            {
                if (commands is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                var resourceNames = commands
                    .Select(p => new KeyValuePair<string, string>(p.LanguageId, p.ResourceName)).Distinct().ToArray();
                var resources = await _localeRepository.GetByKeys(resourceNames);
                List<Locale> locales = new List<Locale>();
                foreach (var command in commands)
                {
                    RLocaleStringResource? rLocaleStringResource = resources.FirstOrDefault(p =>
                        p.LanguageId == command.LanguageId && p.ResourceName == command.ResourceName);
                    Locale locale;
                    if (rLocaleStringResource != null)
                    {
                        locale = new Locale(rLocaleStringResource);
                        locale.Change(command);
                    }
                    else
                    {
                        locale = new Locale(command);
                    }

                    locales.Add(locale);
                }

                await _localeRepository.AddOrChange(locales.ToArray());
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse> AddIfNotExist(LocaleInsertCommand[] commands)
        {
            return await ProcessCommand(async (response) =>
            {
                if (commands is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                var resourceNames = commands
                    .Select(p => new KeyValuePair<string, string>(p.LanguageId, p.ResourceName.ToLower())).Distinct()
                    .ToArray();
                var resources = await _localeRepository.GetByKeys(resourceNames);
                List<Locale> locales = new List<Locale>();
                foreach (var command in commands)
                {
                    RLocaleStringResource? rLocaleStringResource = resources.FirstOrDefault(p =>
                        p.LanguageId == command.LanguageId &&
                        p.ResourceName.ToLower() == command.ResourceName.ToLower());
                    if (rLocaleStringResource != null)
                    {
                        continue;
                    }

                    var locale = new Locale(command);
                    locales.Add(locale);
                }

                if (locales.Count > 0)
                {
                    await _localeRepository.Add(locales.ToArray());
                }

                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse> Update(LocaleUpdateCommand locale)
        {
            return await ProcessCommand(async (response) =>
            {
                if (string.IsNullOrEmpty(locale.Id))
                {
                    response.SetFail($"Locale input can't null {locale.Id}!");
                    return;
                }

                var rLocale = await _localeRepository.GetById(locale.Id);
                if (rLocale == null)
                {
                    response.SetFail("Locale not exist");
                    return;
                }

                Locale input = new Locale(rLocale);
                input.Change(locale);
                await _localeRepository.Update(input);
                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse> Delete(LocaleDeleteCommand locale)
        {
            return await ProcessCommand(async (response) =>
            {
                if (string.IsNullOrEmpty(locale.Id))
                {
                    response.SetFail($"Locale input can't null {locale.Id}!");
                    return;
                }

                var rLocale = await _localeRepository.GetById(locale.Id);
                if (rLocale == null)
                {
                    response.SetFail("Locale not exist");
                    return;
                }

                var input = new Locale(rLocale);
                await _localeRepository.Delete(input);
                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse<RLocaleStringResource[]>> GetByLanguageId(
            LocaleStringResourceGetByLanguageIdQuery query)
        {
            return await ProcessCommand<RLocaleStringResource[]>(async (response) =>
            {
                RLocaleStringResource[] locales =
                    await _localeRepository.GetByLanguageId(query.ObjectId);
                response.Data = locales;
                response.SetSuccess();
            });
        }

        [AllowAnonymous]
        public async Task<BaseCommandResponse<string[]>> CheckInValidLocaleInsert(LocaleInsertCommand[] locales)
        {
            return await ProcessCommand<string[]>(async (response) =>
            {
                if (locales is not { Length: > 0 })
                {
                    response.SetSuccess();
                    return;
                }

                List<string> invalids = new List<string>();
                foreach (var lo in locales)
                {
                    var existLo = await _localeRepository.GetByDoubleKeys(lo.LanguageId, lo.ResourceName);
                    if (existLo != null)
                    {
                        invalids.Add(lo.Id);
                    }
                }

                response.Data = invalids.ToArray();
                response.SetSuccess();
            });
        }

        public async Task<BaseCommandResponse<RLocaleStringResource[]>> GetByKeys(
            LocaleStringResourceGetByKeysQuery query)
        {
            return await ProcessCommand<RLocaleStringResource[]>(async (response) =>
            {
                var resourceNames = query.Keys
                    .Select(p => new KeyValuePair<string, string>(query.LanguageId, p.AsEmpty().ToLower())).Distinct()
                    .ToArray();
                var resources = await _localeRepository.GetByKeys(resourceNames);
                List<RLocaleStringResource> localeStringResources = new List<RLocaleStringResource>();
                foreach (var resource in resources)
                {
                    localeStringResources.Add(resource);
                }

                foreach (var key in query.Keys)
                {
                    if (resources.All(p => !string.Equals(p.ResourceName.AsEmpty(), key.AsEmpty(),
                            StringComparison.CurrentCultureIgnoreCase)))
                    {
                        localeStringResources.Add(new RLocaleStringResource()
                        {
                            LanguageId = query.LanguageId,
                            ResourceName = key,
                            ResourceValue = key,
                        });
                    }
                }

                response.Data = localeStringResources.ToArray();
                response.SetSuccess();
            });
        }
    }
}
