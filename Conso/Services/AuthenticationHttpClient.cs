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
    Task<LoginResponse?> GetJwt(string username, string password);
}

public class AuthenticationHttpClient : IAuthenticationHttpClient
{
    private readonly ILogger<AuthenticationHttpClient> logger;
    private readonly HttpClient httpClient;

    private static class LogMessage
    {
        // Conso.Services.AuthenticationHttpClient:JWT retrieved
        internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedContent = LoggerMessage.Define<LoginResponse?>(
            LogLevel.Information, new EventId(292512, "JWT retrieved"), "JWT {jwt}");

        // Conso.Services.AuthenticationHttpClient:Retrieve null LoginResponse
        internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedNullLoginResponse = LoggerMessage.Define<LoginResponse?>(
            LogLevel.Information, new EventId(301829, "Retrieve null LoginResponse"), "LoginResponse {LoginResponse}");
    }

    public AuthenticationHttpClient(ILogger<AuthenticationHttpClient>? logger, HttpClient httpClient, 
        IOptionsMonitor<HttpClientSetting> optionsMonitor)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));

        this.httpClient = httpClient;
        
        var httpClientSetting = optionsMonitor.Get(HttpClientName.Authentication);

        httpClientSetting.EnsureIsValid();

        httpClient.BaseAddress = new Uri(httpClientSetting.BaseAddress);
    }

    public async Task<LoginResponse?> GetJwt(string username, string password)
    {
        var responseMessage = await httpClient.PostAsync("/api/authentication", JsonContent.Create<LoginRequest>(new LoginRequest {
            Username = username,
            Password = password
        }, mediaType: new MediaTypeHeaderValue(MediaTypeNames.Application.Json)));

        responseMessage.EnsureSuccessStatusCode();

        var response = await responseMessage.Content.ReadFromJsonAsync<LoginResponse>();

        if (response == null) 
        {
            LogMessage.RetrievedNullLoginResponse(logger, response, null);
            return response;
        }

        LogMessage.RetrievedContent(logger, response, null);
        return response;
    }
}
