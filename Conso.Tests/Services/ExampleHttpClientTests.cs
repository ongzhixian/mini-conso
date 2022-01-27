using Conso.Models;
using Conso.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Conso.Tests.Services;

[TestClass()]
public class ExampleHttpClientTests
{
    private Mock<ILogger<ExampleHttpClient>> mockLogger = new();
    private Mock<IOptionsMonitor<HttpClientSetting>> mockOptionsMonitor = new();
    private Mock<HttpMessageHandler> mockHttpMessageHandler = new();
    private Mock<IBearerTokenService> mockBearerTokenService = new();

    [TestInitialize]
    public void BeforeEachTest()
    {
        mockLogger = new Mock<ILogger<ExampleHttpClient>>();
        mockOptionsMonitor = new Mock<IOptionsMonitor<HttpClientSetting>>();
        mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockBearerTokenService = new Mock<IBearerTokenService>();
    }

    [TestMethod()]
    public void ExampleHttpClientTest()
    {
        mockOptionsMonitor.Setup(m => m.Get("HttpClients:ExampleWeatherForecast")).Returns(new HttpClientSetting
        {
            BaseAddress = "https://localhost:7001"
        });

        ExampleHttpClient client = new(mockLogger.Object, new HttpClient(), mockOptionsMonitor.Object, mockBearerTokenService.Object);

        Assert.IsNotNull(client);
    }

    [ExcludeFromCodeCoverage]
    [TestMethod()]
    public void ExampleHttpClientNullLoggerTest()
    {
        mockOptionsMonitor.Setup(m => m.Get("HttpClients:ExampleWeatherForecast")).Returns(new HttpClientSetting
        {
            BaseAddress = "https://localhost:7001"
        });

        ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
            new ExampleHttpClient(null, new HttpClient(), mockOptionsMonitor.Object, mockBearerTokenService.Object)
        );

        Assert.IsNotNull(ex);
        Assert.AreEqual<string>("Value cannot be null. (Parameter 'logger')", ex.Message);
    }

    [ExcludeFromCodeCoverage]
    [TestMethod()]
    public void ExampleHttpClientNullBearerTokenServiceTest()
    {
        mockOptionsMonitor.Setup(m => m.Get("HttpClients:ExampleWeatherForecast")).Returns(new HttpClientSetting
        {
            BaseAddress = "https://localhost:7001"
        });

        ArgumentNullException? ex = Assert.ThrowsException<ArgumentNullException>(() =>
            new ExampleHttpClient(mockLogger.Object, new HttpClient(), mockOptionsMonitor.Object, null)
        );

        Assert.IsNotNull(ex);
        Assert.AreEqual<string>("Value cannot be null. (Parameter 'bearerTokenService')", ex.Message);
    }

    [TestMethod()]
    public async Task GetWeatherForcastAsyncTestAsync()
    {
        mockOptionsMonitor.Setup(m => m.Get("HttpClients:ExampleWeatherForecast")).Returns(new HttpClientSetting
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
                Content = new StringContent("SomeContent")
            });

        ExampleHttpClient client = new(mockLogger.Object, new HttpClient(mockHttpMessageHandler.Object), mockOptionsMonitor.Object, mockBearerTokenService.Object);

        var result = await client.GetWeatherForcastAsync();

        Assert.IsNotNull(client);
        Assert.AreEqual<string>("SomeContent", result);
    }
}
