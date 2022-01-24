using Conso.Models;
using Conso.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Conso.Tests;

[TestClass]
public class ExampleServiceTests
{
    Mock<ILogger<ExampleService>> mockLogger = new Mock<ILogger<ExampleService>>();
    Mock<IOptions<ApplicationSetting>> mockOptions = new Mock<IOptions<ApplicationSetting>>();
    Mock<IExampleHttpClient> mockHttpClient = new Mock<IExampleHttpClient>();

    [TestInitialize]
    public void BeforeEachTest()
    {
        mockLogger = new Mock<ILogger<ExampleService>>();
        mockOptions = new Mock<IOptions<ApplicationSetting>>();
        mockHttpClient = new Mock<IExampleHttpClient>();

    }
    [TestMethod]
    public async Task TestMethod1Async()
    {
        mockOptions.Setup(m => m.Value).Returns(new ApplicationSetting
        {
            Version = "0.0.0",
            RunType = "Console"
        });
        mockHttpClient.Setup(m => m.GetWeatherForcastAsync()).ReturnsAsync("somestring");

        ExampleService service = new ExampleService(mockLogger.Object, mockOptions.Object, mockHttpClient.Object);

        await service.DoWorkAsync();

        mockHttpClient.Verify(x => x.GetWeatherForcastAsync(), Times.Once);
    }
}