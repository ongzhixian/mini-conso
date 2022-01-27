using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Mini.Common.Responses;

namespace Conso.Services
{
    public interface IBearerTokenService
    {
        Task<string> GetBearerTokenAsync();
        void Set(string token, TimeSpan storageTimeSpan);
    }

    public class BearerTokenService : IBearerTokenService
    {
        private readonly ILogger<BearerTokenService> logger;
        private readonly IMemoryCache cache;
        private readonly IAuthenticationHttpClient authenticationHttpClient;

        private static class LogMessage
        {
            // Conso.Services.BearerTokenService:Store JWT to cache
            internal readonly static Action<ILogger, Exception?> StoreJwtToCache = LoggerMessage.Define(
                LogLevel.Information, new EventId(268598, "Stored JWT to cache"), "Store JWT to cache");

            // Conso.Services.BearerTokenService:Retrieved null LoginResponse
            internal readonly static Action<ILogger, LoginResponse?, Exception?> RetrievedNullLoginResponse = LoggerMessage.Define<LoginResponse?>(
                LogLevel.Information, new EventId(240405, "Retrieved null LoginResponse"), "LoginResponse {LoginResponse}");
        }

        public BearerTokenService(ILogger<BearerTokenService>? logger, IMemoryCache? cache, IAuthenticationHttpClient? authenticationHttpClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.authenticationHttpClient = authenticationHttpClient ?? throw new ArgumentNullException(nameof(authenticationHttpClient));
        }

        public async Task<string> GetBearerTokenAsync()
        {
            var jwt = (string)cache.Get(CacheKey.BEARER_TOKEN);

            while (string.IsNullOrWhiteSpace(jwt)) {
                var response = await authenticationHttpClient.GetJwt("someUsername", "somePassword");

                if (response == null) {
                    LogMessage.RetrievedNullLoginResponse(logger, response, null);
                    return string.Empty;
                }

                // Expire the bearer token 10 minutes before it expired

                jwt = response.Jwt;
                Set(jwt, response.ExpiryDateTime - DateTime.UtcNow.AddMinutes(10));
            }

            return jwt;
        }

        public void Set(string token, TimeSpan storageTimeSpan)
        {
            _ = cache.Set(CacheKey.BEARER_TOKEN, token, storageTimeSpan);
            LogMessage.StoreJwtToCache(logger, null);
        }
    }
}
