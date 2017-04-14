using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureRepositories
{
    public interface ILogRepository
    {
        Task InsertAsync(LogEntity olapEntity);
        Task<IEnumerable<LogEntity>> GetRange(DateTime dateFrom, DateTime dateTo, string clientId);
    }
}
