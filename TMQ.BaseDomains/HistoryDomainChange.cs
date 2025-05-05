using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseDomains
{
    public class HistoryDomainChange
    {
        public HistoryDomainChange(int priority, string domain, bool isSystem)
        {
            Priority = priority;
            Domain = domain;
            IsSystem = isSystem;
        }
        public int Priority { get; set; }
        public string Domain { get; private set; }
        public bool IsSystem { get; set; }
    }
}
