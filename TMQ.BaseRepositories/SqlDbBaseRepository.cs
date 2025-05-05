using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseDomains;
using Z.Dapper.Plus;

namespace TMQ.BaseRepositories
{
    public abstract class SqlDbBaseRepository<T>(IDbConnectionFactory dbConnectionFactory)
    : ISqlDbBaseRepository<T> where T : BaseDomain
    {
        protected readonly IDbConnectionFactory DbConnectionFactory = dbConnectionFactory;

        public async Task Add(T obj)
        {
            await DbConnectionFactory.WithConnection(async connection => { await connection.BulkInsertAsync(obj); });
        }

        public async Task Change(T obj)
        {
            await DbConnectionFactory.WithConnection(async (connection) => { await connection.BulkUpdateAsync(obj); });
        }
    }
}
