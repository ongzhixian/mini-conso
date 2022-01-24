using Conso.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics.CodeAnalysis;

namespace Conso.Services;

[ExcludeFromCodeCoverage]
internal static class AppStartupService
{
    internal static void ConfigureApp(Func<Action<HostBuilderContext, IConfigurationBuilder>, IHostBuilder> configureAppConfiguration) =>
        configureAppConfiguration.Invoke((hostingContext, configurationBuilder) =>
        {
            configurationBuilder.SetBasePath(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
            configurationBuilder.AddJsonFile("appsecrets.json", optional: true, reloadOnChange: true);

            if (hostingContext.HostingEnvironment.IsDevelopment())
            {
                // Print any configuration values we want to see here after building configuration

                var configuration = configurationBuilder.Build();

                foreach (var item in configuration.GetSection("Application:Debug:ConfigKeys").GetChildren().Select(r => r.Value))
                {
                    Console.WriteLine($"ConfigKey: [{item}] = [{configuration[item]}]");
                }
            }
        });

    internal static void ConfigureServices(Func<Action<HostBuilderContext, IServiceCollection>, IHostBuilder> configureServices) =>
        configureServices.Invoke((host, services) =>
        {
            RegisterConfigurationInstances(host, services);

            // RegisterHttpClients(services)

            services.AddHttpClient();
            services.AddHttpClient<IExampleHttpClient, ExampleHttpClient>();

            // RegisterCaching(services)
            //services.AddMemoryCache(); // default IMemoryCache implementation

            // RegisterServices(services)
            services.AddSingleton<ExampleService>();

            // RegisterHostedServices(services)
            // N/A

        });

    private static void RegisterConfigurationInstances(HostBuilderContext host, IServiceCollection services)
    {
        services.Configure<ApplicationSetting>(host.Configuration.GetSection("Application"));

        services.Configure<HttpClientSetting>("HttpClients:ExampleWeatherForecast", host.Configuration.GetSection("HttpClients:ExampleWeatherForecast"));

    }
}