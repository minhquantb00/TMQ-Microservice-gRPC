using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.GrpcClient
{
    public delegate T GrpcClientResolver<out T>();
}
