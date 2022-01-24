using Conso.Models;
using Conso.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
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


    [TestInitialize]
    public void BeforeEachTest()
    {
        mockLogger = new Mock<ILogger<ExampleHttpClient>>();
        mockOptionsMonitor = new Mock<IOptionsMonitor<HttpClientSetting>>();
        mockHttpMessageHandler = new Mock<HttpMessageHandler>();
    }

    [TestMethod()]
    public void ExampleHttpClientTest()
    {
        mockOptionsMonitor.Setup(m => m.Get("HttpClients:ExampleWeatherForecast")).Returns(new HttpClientSetting
        {
            BaseAddress = "https://localhost:7001"
        });

        ExampleHttpClient client = new(mockLogger.Object, new HttpClient(), mockOptionsMonitor.Object);

        Assert.IsNotNull(client);
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

        ExampleHttpClient client = new(mockLogger.Object, new HttpClient(mockHttpMessageHandler.Object), mockOptionsMonitor.Object);

        var result = await client.GetWeatherForcastAsync();

        Assert.IsNotNull(client);
        Assert.AreEqual<string>("SomeContent", result);
    }
}
