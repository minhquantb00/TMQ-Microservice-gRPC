using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;

namespace TMQ.BaseRepositories
{
    public interface ISqlDbBaseRepository<in T> where T : BaseDomain
    {
        Task Add(T obj);
        Task Change(T obj);
    }
}
