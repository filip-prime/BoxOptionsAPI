using System;
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
    }
}
