using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseReadModels;
using TMQ.SystemDomains;
using TMQ.SystemReadModels;

namespace TMQ.SystemRepository
{
    public interface IZonePositionHistoryRepository
    {
        Task Add(ZonePositionHistory[] histories);
        Task<RZonePositionHistory[]> Gets(string zoneId, RefSqlPaging sqlPaging, string changeId, bool isRoot);
    }
}
