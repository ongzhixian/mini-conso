using Microsoft.VisualStudio.TestTools.UnitTesting;
using Conso.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using Mini.Common.Responses;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Options;
using Conso.Models;

namespace Conso.Services.Tests
{
    [TestClass()]
    public class BearerTokenServiceTests
    {
        private Mock<ILogger<BearerTokenService>> mockLogger = new();
        private Mock<IMemoryCache> mockCache = new();
        private Mock<IAuthenticationHttpClient> mockClient = new();
        private Mock<IOptions<UserCredentialSetting>> mockUserCredentialSetting = new();

        [TestInitialize]
        public void BeforeEachTest()
        {
            mockLogger = new Mock<ILogger<BearerTokenService>>();
            mockCache = new Mock<IMemoryCache>();
            mockClient = new Mock<IAuthenticationHttpClient>();
            mockUserCredentialSetting = new();
        }

        [TestMethod()]
        public void BearerTokenServiceTest()
        {
            BearerTokenService service = new(mockLogger.Object, mockCache.Object, mockClient.Object, mockUserCredentialSetting.Object);
            
            Assert.IsNotNull(service);
        }

        [ExcludeFromCodeCoverage]
        [TestMethod()]
        public void BearerTokenServiceNullLoggerTest()
        {
            ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
                new BearerTokenService(null, mockCache.Object, mockClient.Object, mockUserCredentialSetting.Object)
            );

            Assert.IsNotNull(ex);
            Assert.AreEqual<string>("Value cannot be null. (Parameter 'logger')", ex.Message);
        }

        [ExcludeFromCodeCoverage]
        [TestMethod()]
        public void BearerTokenServiceNullClientTest()
        {
            ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
                new BearerTokenService(mockLogger.Object, mockCache.Object, null, mockUserCredentialSetting.Object)
            );

            Assert.IsNotNull(ex);
            Assert.AreEqual<string>("Value cannot be null. (Parameter 'authenticationHttpClient')", ex.Message);
        }

        [ExcludeFromCodeCoverage]
        [TestMethod()]
        public void BearerTokenServiceNullCacheTest()
        {
            ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
                new BearerTokenService(mockLogger.Object, null, mockClient.Object, mockUserCredentialSetting.Object)
            );

            Assert.IsNotNull(ex);
            Assert.AreEqual<string>("Value cannot be null. (Parameter 'cache')", ex.Message);
        }


        [TestMethod()]
        public async Task GetBearerTokenAsyncTestAsync()
        {
            mockClient.Setup(m => m.GetJwt(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new LoginResponse()
            {
                Jwt = "someJwt"
            });

            mockUserCredentialSetting.Setup(m => m.Value).Returns(new UserCredentialSetting
            {
                Username = "someUsername",
                Password = "somePassword"
            });

            mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            BearerTokenService service = new(mockLogger.Object, mockCache.Object, mockClient.Object, mockUserCredentialSetting.Object);

            string result = await service.GetBearerTokenAsync();

            Assert.IsNotNull(service);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public async Task GetBearerTokenAsyncNullLoginResponseTestAsync()
        {
            mockClient.Setup(m => m.GetJwt(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync((LoginResponse?)null);

            mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            mockUserCredentialSetting.Setup(m => m.Value).Returns(new UserCredentialSetting
            {
                Username = "someUsername",
                Password = "somePassword"
            });

            BearerTokenService service = new(mockLogger.Object, mockCache.Object, mockClient.Object, mockUserCredentialSetting.Object);

            string result = await service.GetBearerTokenAsync();

            Assert.IsNotNull(service);
            Assert.IsNotNull(result);
        }

        [TestMethod()]
        public void SetTest()
        {
            mockClient.Setup(m => m.GetJwt(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(new LoginResponse()
            {
                Jwt = "someJwt"
            });

            mockCache.Setup(m => m.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>);

            BearerTokenService service = new(mockLogger.Object, mockCache.Object, mockClient.Object, mockUserCredentialSetting.Object);

            service.Set("someToken", new TimeSpan(1, 0, 0));

            Assert.IsNotNull(service);
        }
    }
}