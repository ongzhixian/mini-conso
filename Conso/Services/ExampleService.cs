using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conso.Services;

public class ExampleService
{
    private readonly ILogger<ExampleService> logger;
    private readonly ApplicationSetting options;
    private readonly IExampleHttpClient client;

    private static class LogMessage
    {
        internal readonly static Action<ILogger, string, Exception?> Version = LoggerMessage.Define<string>(
            LogLevel.Information, new EventId(227865, "Version display"), "Version {version}");

        internal readonly static Action<ILogger, string, Exception?> RunType = LoggerMessage.Define<string>(
            LogLevel.Information, new EventId(344333, "RunType display"), "Version {runType}");

        internal readonly static Action<ILogger, string, Exception?> Forecast = LoggerMessage.Define<string>(
            LogLevel.Information, new EventId(208780, "Weather forecast display"), "Forecast is {forecast}");
    }

    public ExampleService(ILogger<ExampleService> logger, IOptions<ApplicationSetting> options, IExampleHttpClient client)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task DoWorkAsync()
    {
        // LogMessage.Version(logger, options.Version, null);

        // LogMessage.RunType(logger, options.RunType, null);

        var forecast = await client.GetWeatherForcastAsync();

        LogMessage.Forecast(logger, forecast, null);
    }
}
