using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conso.Services;

public class AuthenticationService
{
    private readonly ILogger<AuthenticationService> logger;
    private readonly IAuthenticationHttpClient client;

    private static class LogMessage
    {
        // Conso.Services.AuthenticationService:Retrieved JWT
        internal readonly static Action<ILogger, Exception?> RetrievedJwt = LoggerMessage.Define(
            LogLevel.Information, new EventId(355410, "Retrieved JWT"), "Retrieved JWT");
    }

    public AuthenticationService(ILogger<AuthenticationService>? logger, IAuthenticationHttpClient? client)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async Task DoWorkAsync()
    {

        
        await client.GetJwt("asd", "sdsa");
        LogMessage.RetrievedJwt(logger, null);
    }

}
