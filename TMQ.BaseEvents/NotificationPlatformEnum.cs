using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.BaseEvents
{
    public enum NotificationPlatformEnum
    {
        All = 1,
        Mobile = 2,
        WebAdmin = 4,
        Email = 8,
        SMS = 16,
        WebCustomer = 32,
    }
}
