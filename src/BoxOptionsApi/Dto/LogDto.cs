using AzureRepositories;

namespace BoxOptionsApi.Dto
{
    public class LogDto : ILogEntity
    {
        public string ClientId { get; set; }
        public string EventCode { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
    }
}
