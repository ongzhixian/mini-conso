using Conso.Models;
using Conso.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Threading.Tasks;

namespace Conso.Tests.Services;

[TestClass]
public class ExampleServiceTests
{
    private Mock<IExampleHttpClient> mockHttpClient = new();
    private Mock<ILogger<ExampleService>> mockLogger = new();
    private Mock<IOptions<ApplicationSetting>> mockOptions = new();

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

        await new ExampleService(mockLogger.Object, mockOptions.Object, mockHttpClient.Object).DoWorkAsync();

        mockHttpClient.Verify(x => x.GetWeatherForcastAsync(), Times.Once);
    }
}