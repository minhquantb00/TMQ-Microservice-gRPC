using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    public enum NotifyMessageType
    {
        All = 1,
        Group = 2,
        Client = 3,
        AllClientByUsers = 5,
        Department = 6,
        Company = 7,
    }
}
