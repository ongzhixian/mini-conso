using Conso.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AppStartup = Conso.Services.AppStartupService;

var builder = Host.CreateDefaultBuilder(args);

Console.WriteLine("Configuring application options...");

AppStartup.ConfigureApp(builder.ConfigureAppConfiguration);

AppStartup.ConfigureServices(builder.ConfigureServices);


builder.ConfigureServices((host, services) =>
{
    IHostEnvironment hostingEnvironment = host.HostingEnvironment;
    
    if (hostingEnvironment.IsDevelopment())
    {
        Console.WriteLine("IsDevelopment");
    }

    Console.WriteLine("Application:Version [{0}]", host.Configuration["Application:Version"]);
    Console.WriteLine("Application:Name    [{0}]", host.Configuration["Application:Name"]);

    //services.Configure<MongoDbSetting>(host.Configuration.GetSection("mongodb:minitools"));
    



    // Add configuration

    // services.AddHttpClient<UserAuthenticationApiService>();




});



Console.WriteLine("Buiding application host...");

IHost? host = builder.Build();

var logger = host.Services.GetRequiredService<ILogger<Program>>();

logger.LogInformation("Running application...");

var a3 = host.Services.GetRequiredService<IOptions<MongoDbSetting>>();

Console.WriteLine(a3);
// if (host.Environment.IsDevelopment())
// {
// }

//host.Run();



