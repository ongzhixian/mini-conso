using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Conso.Services;

internal static class AppStartupService
{
    internal static void ConfigureApp(Func<Action<HostBuilderContext, IConfigurationBuilder>, IHostBuilder> configureAppConfiguration)
    {
        configureAppConfiguration.Invoke((hostingContext, configurationBuilder) =>
        {
            configurationBuilder.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            configurationBuilder.AddJsonFile("appsecrets.json", optional: true, reloadOnChange: true);

            //IHostEnvironment hostingEnvironment = hostingContext.HostingEnvironment;
            //var configuration = configurationBuilder.Build();
            //Console.WriteLine("a {0}", configuration["MongoDb:ConnectionString"]);

        });
    }

    internal static void ConfigureServices(Func<Action<HostBuilderContext, IServiceCollection>, IHostBuilder> configureServices)
    {
        configureServices.Invoke((host, services) =>
        {
            RegisterConfigurationInstances(host, services);

            // # RegisterHttpClients(services)

            //services.AddHttpClient()
            //services.AddHttpClient<OandaApiService>();

            // # RegisterCaching(services)

            //services.AddMemoryCache(); // default IMemoryCache implementation

            // # RegisterServices(services)

            //services.AddSingleton<IMyService, MyService>()

            // # RegisterHostedServices(services)

            //services.AddHostedService<ExampleHostedService>()
            //services.AddHostedService<ExampleBackgroundService>()
            //services.AddHostedService<ExampleEventEmitterService>()

            // # Azure (examples)

            //services.AddHostedService<PubSubConsumerService>()
            //services.AddHostedService<PubSubPublisherService>()
            //services.AddHostedService<QueuePublisherService>()
            //services.AddHostedService<QueueConsumerService>()
            //services.AddHostedService<StorageTableService>()
            //services.AddHostedService<StorageBlobService>()

            // # Mongo (examples)

            //services.AddHostedService<MongoDbMiniToolsService>()

            // # Oanda 

            //services.AddHostedService<TradeStrategyService>()

        });
    }

    private static void RegisterConfigurationInstances(HostBuilderContext host, IServiceCollection services)
    {
        //services.Configure<MongoDbSetting>(host.Configuration.GetSection("mongodb:minitools"));
        services.Configure<MongoDbSetting>(host.Configuration.GetSection("MongoDb"));

        // builder.Services.Configure<MongoDbSettings>(
        //     "mongodb:minitools",
        //     builder.Configuration.GetSection("mongodb:minitools")
        //     );

        // if (builder.Environment.IsDevelopment() && builder.Configuration.GetValue<bool>("Application:Startup:DumpDebugInfo"))
        // {
        //     Console.WriteLine("# Development debug info:");
        //     Console.WriteLine("Jwt:ValidAudience    [{0}]", builder.Configuration["Jwt:ValidAudience"]);
        //     Console.WriteLine("Jwt:ValidIssuer      [{0}]", builder.Configuration["Jwt:ValidIssuer"]);
        //     Console.WriteLine("Jwt:SecretKey        [{0}]", builder.Configuration["Jwt:SecretKey"]);
        // }
    }
}


public class MongoDbSetting
{
    public string ConnectionString { get; set; } = string.Empty;
}
