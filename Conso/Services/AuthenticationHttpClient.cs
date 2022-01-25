using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;

namespace Conso.Services;

public interface IAuthenticationHttpClient
{

    Task<string> GetJwt(string username, string password);
}

public class AuthenticationHttpClient : IAuthenticationHttpClient
{
    private readonly ILogger<ExampleHttpClient> logger;
    private readonly HttpClient httpClient;

    private static class LogMessage
    {
        internal readonly static Action<ILogger, string, Exception?> RetrievedContent = LoggerMessage.Define<string>(
            LogLevel.Information, new EventId(285285, "Weather forcast retrieved"), "Weather forecast {weatherForecast}");
    }

    public AuthenticationHttpClient(ILogger<ExampleHttpClient> logger, HttpClient httpClient, IOptionsMonitor<HttpClientSetting> optionsMonitor)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.httpClient = httpClient;
        
        HttpClientSetting httpClientSetting = optionsMonitor.Get("HttpClients:Authentication") ?? throw new NullReferenceException(nameof(httpClientSetting));
        
        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);
    }

    public async Task<string> GetJwt(string username, string password)
    {
        var responseMessage = await httpClient.PostAsync("/api/authentication", JsonContent.Create<LoginRequest>(new LoginRequest {
            Username = "dev",
            Password = "dev"
        }, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage.EnsureSuccessStatusCode();

        var content = await responseMessage.Content.ReadAsStringAsync();

        LogMessage.RetrievedContent(logger, content, null);
        
        return content;
    }
}

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
