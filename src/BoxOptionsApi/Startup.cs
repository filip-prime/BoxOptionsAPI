using System;
using System.Net.Http;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AzureRepositories;
using AzureStorage.Tables;
using BoxOptionsApi.Other;
using BoxOptionsApi.Other.Exceptions;
using Common.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.Swagger.Model;

namespace BoxOptionsApi
{
    public class Startup
    {
        private IHostingEnvironment Environment { get; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            Environment = env;

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc(o =>
            {
                o.Filters.Add(new HandleAllExceptionsFilterFactory());
            });

            services.AddSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Api"
                });
                options.DescribeAllEnumsAsStrings();
            });

            var logAggregate = new LogAggregate().AddLogger(new LogToConsole());
            var log = logAggregate.CreateLogger();

            var appSettings = Environment.IsEnvironment("Development") ? Configuration.Get<ApplicationSettings>() : LoadSettings(Configuration.GetValue<string>("SETTINGS_URL"), log);
            
            if (!ApplicationSettings.IsSettingsValid(appSettings, log, Program.Name))
            {
                throw new ArgumentException();   
            }

            log = ConfigureLog(appSettings, logAggregate, log);

            var ioc = new ContainerBuilder();
            
            ioc.RegisterInstance(appSettings).AsSelf();
            ioc.RegisterInstance(log).As<ILog>();
            ioc.RegisterInstance(new LogRepository(new AzureTableStorage<LogEntity>(appSettings.BoxOptionsApi.ConnectionStrings.BoxOptionsApiStorage, "ClientEventLogs", log))).As<ILogRepository>();

            ioc.Populate(services);
            return new AutofacServiceProvider(ioc.Build());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseMvc();

            app.UseSwagger();
            app.UseSwaggerUi("swagger");
        }

        private static ApplicationSettings LoadSettings(string url, ILog log)
        {
            log.Info("Loading app settings from web-site.", Program.Name);
            HttpClient webClient = new HttpClient();
            string settingsJson = webClient.GetStringAsync(url).Result;
            return JsonConvert.DeserializeObject<ApplicationSettings>(settingsJson);
        }

        private static ILog ConfigureLog(ApplicationSettings settings, LogAggregate logAggregate, ILog log)
        {
            log.Info("Initializing azure/slack logger.", Program.Name);
            var services = new ServiceCollection(); // only used for azure logger
            logAggregate.ConfigureAzureLogger(services, Program.Name, settings);
            return logAggregate.CreateLogger();
        }
    }
}
