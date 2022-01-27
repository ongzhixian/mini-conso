using Conso.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;

namespace Conso.Services;

public interface IExampleHttpClient
{
    Task<string> GetWeatherForcastAsync();
}

public class ExampleHttpClient : IExampleHttpClient
{
    private readonly ILogger<ExampleHttpClient> logger;
    private readonly HttpClient httpClient;
    private readonly IBearerTokenService bearerTokenService;

    private static class LogMessage
    {
        // Conso.Services.ExampleHttpClient:Weather forcast retrieved
        internal readonly static Action<ILogger, string, Exception?> RetrievedContent = LoggerMessage.Define<string>(
            LogLevel.Information, new EventId(285285, "Weather forcast retrieved"), "Weather forecast {weatherForecast}");
    }

    public ExampleHttpClient(ILogger<ExampleHttpClient>? logger, HttpClient httpClient, 
        IOptionsMonitor<HttpClientSetting> optionsMonitor, IBearerTokenService? bearerTokenService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        this.httpClient = httpClient;

        var httpClientSetting = optionsMonitor.Get(HttpClientName.WeatherForecast);

        httpClientSetting.EnsureIsValid();

        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        this.bearerTokenService = bearerTokenService ?? throw new ArgumentNullException(nameof(bearerTokenService));
    }

    public async Task<string> GetWeatherForcastAsync()
    {
        var bearerToken = await bearerTokenService.GetBearerTokenAsync();

        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

        var responseMessage = await httpClient.GetAsync("/WeatherForecast");

        responseMessage.EnsureSuccessStatusCode();

        var content = await responseMessage.Content.ReadAsStringAsync();

        LogMessage.RetrievedContent(logger, content, null);
        
        return content;
    }
}
