using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using AzureStorage.Tables;

namespace AzureRepositories
{
    public class LogRepository : ILogRepository
    {
        private readonly AzureTableStorage<LogEntity> _storage;

        public LogRepository(AzureTableStorage<LogEntity> storage)
        {
            _storage = storage;
        }

        public async Task InsertAsync(LogEntity olapEntity)
        {
            await _storage.InsertAndGenerateRowKeyAsDateTimeAsync(olapEntity, DateTime.UtcNow);
        }

        public async Task<IEnumerable<LogEntity>> GetRange(DateTime dateFrom, DateTime dateTo, string clientId)
        {
            var entities = (await _storage.GetDataAsync(new[] { clientId }, int.MaxValue,
                entity => entity.Timestamp >= dateFrom && entity.Timestamp < dateTo))
                .OrderByDescending(item => item.Timestamp);

            return entities;
        }
    }
}
