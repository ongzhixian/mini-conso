using Conso.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using AppStartup = Conso.Services.AppStartupService;

var builder = Host.CreateDefaultBuilder(args);

Console.WriteLine("Configuring application options...");

AppStartup.ConfigureApp(builder.ConfigureAppConfiguration);

Console.WriteLine("Configuring application services...");

AppStartup.ConfigureServices(builder.ConfigureServices);

Console.WriteLine("Buiding application host...");

IHost? host = builder.Build();

host.Start(); // vs host.Run

var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Running application...");

var service = host.Services.GetRequiredService<ExampleService>();

await service.DoWorkAsync();

logger.LogInformation("Application end.");