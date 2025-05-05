using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.EnumDefine;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;

namespace TMQ.SystemRepository
{
    public interface ILanguageRepository
    {
        public Task<RLanguage[]> Gets();
        public Task Add(Language language);
        public Task Update(Language language);
        public Task<RLanguage?> GetById(string id);
        public Task<RLanguage[]> GetByType(LanguageTypeEnum type);
    }
}
