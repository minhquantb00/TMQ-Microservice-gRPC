using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.EventBus
{
    public interface IMessageHandler
    {
        public string WorkerGroup { get; }
    }
}
