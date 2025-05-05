using System.Data.Common;
using System.Data;
using Microsoft.Data.SqlClient;

namespace TMQ.BaseRepositories
{
    public class DbConnectionFactory(string connectionString) : IDbConnectionFactory
    {
        private async Task<IDbConnection> GetNewConnectionAsync()
        {
            try
            {
                DbConnection dbConnection = new SqlConnection(connectionString);
                await dbConnection.OpenAsync();
                return dbConnection;
            }
            catch (Exception e)
            {
                e.Data["BaseDao.Message-CreateDbConnection"] = "Not new SqlConnection";
                e.Data["BaseDao.ConnectionString"] = connectionString;
                throw;
            }
        }

        public async Task<T> WithConnection<T>(Func<IDbConnection, Task<T>> getData)
        {
            try
            {
                using var dbConnection = await GetNewConnectionAsync();
                return await getData(dbConnection);
            }
            catch (TimeoutException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute TimeoutException";
                throw ex;
            }
            catch (SqlException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute SqlException";
                Console.WriteLine($"Exception Type: {ex.GetType()} - Message: {ex.Message}");
                throw ex;
            }
            catch (Exception ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Exception";
                Console.WriteLine($"Exception Type: {ex.GetType()} - Message: {ex.Message}");
                throw ex;
            }
        }

        public async Task WithConnection(Func<IDbConnection, Task> getData)
        {
            try
            {
                using var dbConnection = await GetNewConnectionAsync();
                await getData(dbConnection);
            }
            catch (TimeoutException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute TimeoutException";
                throw;
            }
            catch (SqlException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute SqlException";
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Exception";
                throw;
            }
        }

        public async Task<T> WithConnection<T>(Func<IDbConnection, IDbTransaction, Task<T>> getData)
        {
            try
            {
                using var dbConnection = await GetNewConnectionAsync();
                using IDbTransaction transaction = dbConnection.BeginTransaction();
                try
                {
                    var result = await getData(dbConnection, transaction);
                    transaction.Commit();
                    return result;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction Exception";
                    throw;
                }
            }
            catch (TimeoutException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction TimeoutException";
                throw;
            }
            catch (SqlException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction SqlException";
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction Exception";
                throw;
            }
        }

        public async Task WithConnection(Func<IDbConnection, IDbTransaction, Task> getData)
        {
            try
            {
                using var dbConnection = await GetNewConnectionAsync();
                using IDbTransaction transaction = dbConnection.BeginTransaction();
                try
                {
                    await getData(dbConnection, transaction);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction Exception";
                    throw;
                }
            }
            catch (TimeoutException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction TimeoutException";
                throw;
            }
            catch (SqlException ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction SqlException";
                throw;
            }
            catch (Exception ex)
            {
                ex.Data["BaseDao.Message-WithConnection"] = "Execute Transaction Exception";
                throw;
            }
        }

        public async Task BulkCopy(DataTable table)
        {
            try
            {
                using var dbConnection = await GetNewConnectionAsync();
                SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)dbConnection,
                    SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.UseInternalTransaction, null)
                {
                    DestinationTableName = table.TableName,
                    BatchSize = 1000,
                };
                foreach (DataColumn tableColumn in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(tableColumn.ColumnName, tableColumn.ColumnName);
                }

                await bulkCopy.WriteToServerAsync(table);
                table.Clear();
            }
            catch (Exception ex)
            {
                ex.Data["BaseDao.Message-BulkCopy"] = "BulkCopy Exception";
                throw;
            }
        }

        public async Task BulkCopy(DataTable table, IDbConnection connection, IDbTransaction transaction)
        {
            try
            {
                SqlBulkCopy bulkCopy = new SqlBulkCopy((SqlConnection)connection, SqlBulkCopyOptions.TableLock,
                    (SqlTransaction)transaction)
                {
                    DestinationTableName = table.TableName,
                    BatchSize = 1000
                };
                foreach (DataColumn tableColumn in table.Columns)
                {
                    bulkCopy.ColumnMappings.Add(tableColumn.ColumnName, tableColumn.ColumnName);
                }

                await bulkCopy.WriteToServerAsync(table);
                table.Clear();
            }
            catch (Exception ex)
            {
                ex.Data["BaseDao.Message-BulkCopy"] = "BulkCopy Exception";
                throw;
            }
        }
    }
}
