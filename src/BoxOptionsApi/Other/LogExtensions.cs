using AzureStorage.Tables;
using Common.Log;
using Lykke.Logs;
using Lykke.SlackNotification.AzureQueue;
using Microsoft.Extensions.DependencyInjection;

namespace BoxOptionsApi.Other
{
    public static class LogExtensions
    {
        public static void ConfigureAzureLogger(this LogAggregate logAggregate, IServiceCollection services, string appName, ApplicationSettings appSettings)
        {
            var log = logAggregate.CreateLogger();
            var slackSender = services.UseSlackNotificationsSenderViaAzureQueue(appSettings.SlackNotifications.AzureQueue, log);
            var azureLog = new LykkeLogToAzureStorage(appName,
                new AzureTableStorage<LogEntity>(appSettings.BoxOptionsApi.ConnectionStrings.LogsConnString, appName.Replace(".", string.Empty) + "Logs", log),
                slackSender);
            logAggregate.AddLogger(azureLog);
        }

        public static void Info(this ILog log, string info, string serviceName)
        {
            log.WriteInfoAsync(serviceName, "Program", string.Empty, info).Wait();
        }
    }
}
