using Microsoft.VisualStudio.TestTools.UnitTesting;
using Conso.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Conso.Models;
using System.Net.Http;
using Moq.Protected;
using System.Threading;
using System.Net;
using System.Text.Json;
using Mini.Common.Responses;
using System.Diagnostics.CodeAnalysis;

namespace Conso.Services.Tests
{
    [TestClass()]
    public class AuthenticationHttpClientTests
    {
        private Mock<ILogger<AuthenticationHttpClient>> mockLogger = new();
        private Mock<HttpMessageHandler> mockHttpMessageHandler = new();
        private Mock<IOptionsMonitor<HttpClientSetting>> mockOptions = new();

        [TestInitialize]
        public void BeforeEachTest()
        {
            mockLogger = new Mock<ILogger<AuthenticationHttpClient>>();
            mockHttpMessageHandler = new();
            mockOptions = new Mock<IOptionsMonitor<HttpClientSetting>>();
        }

        [TestMethod()]
        public void AuthenticationHttpClientTest()
        {
            mockOptions.Setup(m => m.Get(It.IsAny<string>())).Returns(new HttpClientSetting()
            {
                BaseAddress = "https://localhost:7001"
            });

            AuthenticationHttpClient client = new(mockLogger.Object, new HttpClient(), mockOptions.Object);

            Assert.IsNotNull(client);    
        }

        [ExcludeFromCodeCoverage]
        [TestMethod()]
        public void AuthenticationHttpClientNullLoggerTest()
        {
            mockOptions.Setup(m => m.Get(It.IsAny<string>())).Returns(new HttpClientSetting()
            {
                BaseAddress = "https://localhost:7001"
            });

            ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
                new AuthenticationHttpClient(null, new HttpClient(), mockOptions.Object)
                );

            Assert.IsNotNull(ex);
            Assert.AreEqual<string>("Value cannot be null. (Parameter 'logger')", ex.Message);
        }

        [TestMethod()]
        public async Task GetJwtTestAsync()
        {
            mockOptions.Setup(m => m.Get(It.IsAny<string>())).Returns(new HttpClientSetting()
            {
                BaseAddress = "https://localhost:7001"
            });

            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(new LoginResponse()
                    {

                    }))
                });

            AuthenticationHttpClient client = new(mockLogger.Object, new HttpClient(mockHttpMessageHandler.Object), mockOptions.Object);

            LoginResponse? response = await client.GetJwt("someUsername", "somePassword");

            Assert.IsNotNull(response);
        }

        [TestMethod()]
        public async Task GetJwtNullContentTestAsync()
        {
            mockOptions.Setup(m => m.Get(It.IsAny<string>())).Returns(new HttpClientSetting()
            {
                BaseAddress = "https://localhost:7001"
            });

            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize((LoginResponse?)null))
                });

            AuthenticationHttpClient client = new(mockLogger.Object, new HttpClient(mockHttpMessageHandler.Object), mockOptions.Object);

            LoginResponse? response = await client.GetJwt("someUsername", "somePassword");

            Assert.IsNull(response);
        }
    }
}