using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMQ.BaseEvents;

namespace TMQ.EventBus
{
    public class EventStorageRepository : IEventStorageRepository
    {
        private readonly string _connectionString;
        private readonly string _dbName;
        private readonly string _tableName;

        public EventStorageRepository(string connectionString, string dbName, string tableName)
        {
            _connectionString = connectionString;
            _dbName = dbName;
            _tableName = tableName;
        }

        public async Task Add(EventBusMessage message, EventStatusEnum status, string exception)
        {
            if (_connectionString?.Length > 0)
            {
                var client = new MongoClient(_connectionString);
                var database = client.GetDatabase(_dbName);
                string collection = $"{_tableName}{message.CreatedDate:yyyyMMdd}";
                IMongoCollection<EventBusMessage> writeCollection = database.GetCollection<EventBusMessage>(collection);
                await writeCollection.InsertOneAsync(message);
            }
        }
    }
}
