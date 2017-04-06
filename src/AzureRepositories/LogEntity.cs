using Microsoft.WindowsAzure.Storage.Table;

namespace AzureRepositories
{
    public class LogEntity : TableEntity, ILogEntity
    {
        public string ClientId { get; set; }
        public string EventCode { get; set; }
        public string Message { get; set; }
    }
}
