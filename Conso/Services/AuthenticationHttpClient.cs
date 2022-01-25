using Conso.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mini.Common.Requests;
using Mini.Common.Responses;
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
    private readonly ILogger<AuthenticationHttpClient> logger;
    private readonly HttpClient httpClient;
    private readonly IMemoryCache cache;

    private static class LogMessage
    {
        internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedContent = LoggerMessage.Define<LoginResponse?>(
            LogLevel.Information, new EventId(259776, "JWT retrieved"), "JWT {jwt}");
    }

    public AuthenticationHttpClient(ILogger<AuthenticationHttpClient> logger, HttpClient httpClient, IOptionsMonitor<HttpClientSetting> optionsMonitor, IMemoryCache cache)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.httpClient = httpClient;
        
        HttpClientSetting httpClientSetting = optionsMonitor.Get("HttpClients:Authentication") ?? throw new NullReferenceException(nameof(httpClientSetting));
        
        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);

        this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task<string> GetJwt(string username, string password)
    {
        string jwt = (string)cache.Get(CacheKey.JWT);

        var responseMessage = await httpClient.PostAsync("/api/authentication", JsonContent.Create<LoginRequest>(new LoginRequest {
            Username = "dev",
            Password = "dev"
        }, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage.EnsureSuccessStatusCode();

        LoginResponse? response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

        LogMessage.RetrievedContent(logger, response, null);

        TimeSpan xoffset = response.ExpiryDateTime - DateTime.UtcNow.AddMinutes(15);

        string? cacheSetResult = cache.Set(CacheKey.JWT, response.Jwt, xoffset);

        var ass = (string)cache.Get(CacheKey.JWT);

        return response.Jwt;
    }
}
