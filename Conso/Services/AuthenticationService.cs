using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conso.Services;

public class AuthenticationService
{
    private readonly ILogger<AuthenticationService> logger;
    private readonly ApplicationSetting options;
    private readonly IAuthenticationHttpClient client;

    private static class LogMessage
    {
        //internal readonly static Action<ILogger, string, Exception?> Version = LoggerMessage.Define<string>(
        //    LogLevel.Information, new EventId(227865, "Version display"), "Version {version}");

        //internal readonly static Action<ILogger, string, Exception?> RunType = LoggerMessage.Define<string>(
        //    LogLevel.Information, new EventId(344333, "RunType display"), "Version {runType}");

        //internal readonly static Action<ILogger, string, Exception?> Forecast = LoggerMessage.Define<string>(
        //    LogLevel.Information, new EventId(208780, "Weather forecast display"), "Forecast is {forecast}");
    }

    public AuthenticationService(ILogger<AuthenticationService> logger, IOptions<ApplicationSetting> options, IAuthenticationHttpClient client)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task DoWorkAsync()
    {

        var jwt = await client.GetJwt("asd", "sdsa");

        Console.WriteLine("Jwt is {0}", jwt);

        //LogMessage.Forecast(logger, forecast, null);
    }

}
