using Conso.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Conso.Services;

public interface IExampleHttpClient
{
    Task<string> GetWeatherForcastAsync();
}

public class ExampleHttpClient : IExampleHttpClient
{
    private readonly ILogger<ExampleHttpClient> logger;
    private readonly HttpClient httpClient;
    private readonly IMemoryCache cache;

    private static class LogMessage
    {
        internal readonly static Action<ILogger, string, Exception?> RetrievedContent = LoggerMessage.Define<string>(
            LogLevel.Information, new EventId(285285, "Weather forcast retrieved"), "Weather forecast {weatherForecast}");
    }

    public ExampleHttpClient(ILogger<ExampleHttpClient> logger, HttpClient httpClient, IOptionsMonitor<HttpClientSetting> optionsMonitor, IMemoryCache cache)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        
        this.httpClient = httpClient;

        HttpClientSetting httpClientSetting = optionsMonitor.Get("HttpClients:ExampleWeatherForecast") ?? throw new NullReferenceException(nameof(httpClientSetting));
        
        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<string> GetWeatherForcastAsync()
    {
        string jwt = (string)cache.Get(CacheKey.JWT);

        httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwt);

        var responseMessage = await httpClient.GetAsync("/WeatherForecast");

        responseMessage.EnsureSuccessStatusCode();

        var content = await responseMessage.Content.ReadAsStringAsync();

        LogMessage.RetrievedContent(logger, content, null);
        
        return content;
    }
}
