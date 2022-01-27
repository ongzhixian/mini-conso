using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conso.Services;

public class ExampleService
{
    private readonly ILogger<ExampleService> logger;
    private readonly IExampleHttpClient client;

    private static class LogMessage
    {
        // Conso.Services.ExampleService:Retrieved weather forecast
        internal readonly static Action<ILogger, string, Exception?> Forecast = LoggerMessage.Define<string>(
            LogLevel.Information, new EventId(269544, "Retrieved weather forecast"), "Forecast is {forecast}");
    }

    public ExampleService(ILogger<ExampleService>? logger, IExampleHttpClient? client)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task DoWorkAsync()
    {
        var forecast = await client.GetWeatherForcastAsync();

        LogMessage.Forecast(logger, forecast, null);
    }

}
