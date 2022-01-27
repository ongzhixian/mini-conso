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
            services.AddHttpClient<IAuthenticationHttpClient, AuthenticationHttpClient>();

            // RegisterCaching(services)
            
            services.AddMemoryCache(); // default IMemoryCache implementation

            // RegisterServices(services)

            services.AddSingleton<IBearerTokenService, BearerTokenService>();
            services.AddSingleton<ExampleService>();
            services.AddSingleton<AuthenticationService>();

            // RegisterHostedServices(services)
            // N/A

        });


    private static void RegisterConfigurationInstances(HostBuilderContext host, IServiceCollection services)
    {
        services.Configure<ApplicationSetting>(host.Configuration.GetSection("Application"));

        services.Configure<HttpClientSetting>(HttpClientName.WeatherForecast, host.Configuration.GetSection("HttpClients:ExampleWeatherForecast"));

        services.Configure<HttpClientSetting>(HttpClientName.Authentication, host.Configuration.GetSection("HttpClients:Authentication"));

        configureUserCredential(host, services);

        static void configureUserCredential(HostBuilderContext host, IServiceCollection services)
        {
            UserCredentialSetting credential = new();

            var configSection = host.Configuration.GetSection("Application:Debug:UserCredential");
            
            if (configSection.Exists()) {
                configSection.Bind(credential);
            }

            // Note: Username might be read from environment variable
            // Order of priority:
            // Take AppSetting
            // Overwrite with Environment
            // Overwrite with CLI argument

            if (host.Configuration["username"] != null) {
                credential.Username = host.Configuration["username"];
            }

            if (host.Configuration["password"] != null) {
                credential.Username = host.Configuration["password"];
            }

            credential.EnsureIsValid();

            services.Configure<UserCredentialSetting>(cred => {
                cred.Username = credential.Username;
                cred.Password = credential.Password;
            });
        }
    }
}

[ExcludeFromCodeCoverage]
public static class HttpClientName
{
    public const string WeatherForecast = "HttpClients:ExampleWeatherForecast";

    public const string Authentication = "HttpClients:Authentication";
}