using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conso.Services;

public class ExampleService
{
    private readonly ILogger<ExampleService> logger;
    private readonly ApplicationSetting options;
    private readonly IExampleHttpClient client;

    public ExampleService(ILogger<ExampleService> logger, IOptions<ApplicationSetting> options, IExampleHttpClient client)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task DoWorkAsync()
    {
        logger.LogInformation("Version {version}", options.Version);

        logger.LogInformation("RunType {runType}", options.RunType);

        var forecast = await client.GetWeatherForcastAsync();

        logger.LogInformation("Forecast is {forecast}", forecast);
    }
}
