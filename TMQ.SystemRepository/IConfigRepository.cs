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
    public interface IConfigRepository
    {
        Task<RConfig?> GetConfigByKey(string key);
        Task Add(ConfigDomain config);
        Task<bool> Change(ConfigDomain config);
        Task<RConfig[]> Gets(ConfigTypeEnum type);
    }
}
