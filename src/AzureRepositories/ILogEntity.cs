namespace AzureRepositories
{
    public interface ILogEntity
    {
        string ClientId { get; set; }
        string EventCode { get; set; }
        string Message { get; set; }
    }
}
