using System.Threading.Tasks;

namespace AzureRepositories
{
    public interface ILogRepository
    {
        Task InsertAsync(LogEntity olapEntity);
    }
}
