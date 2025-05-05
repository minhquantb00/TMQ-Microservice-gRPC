using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;
using TMQ.SystemCommands.Commands;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;

namespace TMQ.SystemRepository
{
    public interface ILocaleStringResourceRepository
    {
        public Task<RLocaleStringResource[]?> Gets(LocaleSearchCommand command, RefSqlPaging paging);
        public Task Insert(Locale command);
        public Task Add(Locale[] command);
        public Task AddOrChange(Locale[] command);
        public Task Update(Locale command);
        public Task Delete(Locale command);
        public Task<RLocaleStringResource> GetById(string id);
        public Task<RLocaleStringResource[]> GetByLanguageId(string languageId);

        public Task<RLocaleStringResource> GetByDoubleKeys(string languageId, string resource);
        public Task<RLocaleStringResource[]> GetByKeys(KeyValuePair<string, string>[] keys);
    }
}
