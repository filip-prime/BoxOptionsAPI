using Common.Log;
using Lykke.AzureQueueIntegration;

namespace BoxOptionsApi.Other
{
    public class ApplicationSettings
    {

        public SlackNotificationsSettings SlackNotifications { get; set; } = new SlackNotificationsSettings();
        public BoxOptionsApiSettings BoxOptionsApi { get; set; } = new BoxOptionsApiSettings();


        public class BoxOptionsApiSettings
        {
            public ConnectionStringsSettings ConnectionStrings { get; set; } = new ConnectionStringsSettings();
        }

        public class ConnectionStringsSettings
        {
            public string BoxOptionsApiStorage { get; set; }
            public string LogsConnString { get; set; }
        }

        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; } = new AzureQueueSettings();
        }


        public static bool IsSettingsValid(ApplicationSettings settings, ILog log, string serviceName)
        {
            bool isValid = true;

            if (string.IsNullOrEmpty(settings.SlackNotifications.AzureQueue.ConnectionString))
            {
                isValid = false;
                log.Info($"Provide {nameof(settings.SlackNotifications)}.{nameof(settings.SlackNotifications.AzureQueue)}.{nameof(settings.SlackNotifications.AzureQueue.ConnectionString)} value in appsettings", serviceName);
            }

            if (string.IsNullOrEmpty(settings.SlackNotifications.AzureQueue.QueueName))
            {
                isValid = false;
                log.Info($"Provide {nameof(settings.SlackNotifications)}.{nameof(settings.SlackNotifications.AzureQueue)}.{nameof(settings.SlackNotifications.AzureQueue.QueueName)} value in appsettings", serviceName);
            }

            if (string.IsNullOrEmpty(settings.BoxOptionsApi.ConnectionStrings.BoxOptionsApiStorage))
            {
                isValid = false;
                log.Info($"Provide {nameof(settings.BoxOptionsApi)}.{nameof(settings.BoxOptionsApi.ConnectionStrings)}.{nameof(settings.BoxOptionsApi.ConnectionStrings.BoxOptionsApiStorage)} value in appsettings", serviceName);
            }

            if (string.IsNullOrEmpty(settings.BoxOptionsApi.ConnectionStrings.LogsConnString))
            {
                isValid = false;
                log.Info($"Provide {nameof(settings.BoxOptionsApi)}.{nameof(settings.BoxOptionsApi.ConnectionStrings)}.{nameof(settings.BoxOptionsApi.ConnectionStrings.LogsConnString)} value in appsettings", serviceName);
            }

            return isValid;
        }
    }
}
