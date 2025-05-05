using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    public enum EventStatusEnum
    {
        New = 0,
        Success = 1,
        Fail = -1,
        Retry = 2
    }
}
